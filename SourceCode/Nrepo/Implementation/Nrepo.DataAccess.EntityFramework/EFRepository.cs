using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Nrepo.Data;
using Nrepo.DataAccess.Internal;
using Nrepo.Internal;

namespace Nrepo.DataAccess.EntityFramework
{
	/// <summary>
	/// Represents the repository implementation using the Entity Framework.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public abstract class EFRepository<TEntity> : Repository<TEntity>
		where TEntity : class
	{
		#region Protected Methods

		/// <summary>
		/// Setups query filtering for the <see cref="Get(GetOperationParameters)"/> operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="query">The query.</param>
		/// <param name="expression">The expression.</param>
		/// <returns>The resulting query.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="query"/> is null.</exception>
		protected virtual IQueryable<TEntity> SetupGetQueryFiltering(DbContext context,
			IQueryable<TEntity> query, QueryFilterExpression expression)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(query, "query");

			if (ReflectionHelper.IsInherited<ISoftDeletable, TEntity>())
			{
				var param = Expression.Parameter(typeof(TEntity));

				query = (IQueryable<TEntity>)ExpressionBuilder.Where(query, ExpressionBuilder.Equals(param,
					ReflectionHelper.GetPropertyName<ISoftDeletable, bool>(e => e.IsDeleted), false), param);
			}

			if (expression != null)
			{
				var interpreter = CreateQueryFilterInterpreter();

				var param = Expression.Parameter(typeof(TEntity));

				query = (IQueryable<TEntity>)ExpressionBuilder.Where(
					query, interpreter.Interpret(expression, param), param);
			}

			return query;
		}

		/// <summary>
		/// Setups query sorting for the <see cref="Get(GetOperationParameters)"/> operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="query">The query.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>The resulting query.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="query"/> is null.</exception>
		protected virtual IQueryable<TEntity> SetupGetQuerySorting(DbContext context,
			IQueryable<TEntity> query, QuerySortingParameters parameters)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(query, "query");

			if (parameters == null || parameters.SortingType == SortingType.None)
			{
				//If the query is not ordered, the the Skip method during paging throws the
				//System.NotSupportedException, because it can be only used with sorted queries
				//By default, sort by the first (or the single) part of the primary key
				return SetupGetQuerySorting(context, query, new QuerySortingParameters()
				{
					SortBy = GetPrimaryKeyNames(context, typeof(TEntity))[0],
					SortingType = SortingType.Ascending
				});
			}

			var param = Expression.Parameter(typeof(TEntity));

			switch (parameters.SortingType)
			{
				case SortingType.Ascending:
					return (IQueryable<TEntity>)ExpressionBuilder.OrderByAscending(
						param, query, parameters.SortBy);
				case SortingType.Descending:
					return (IQueryable<TEntity>)ExpressionBuilder.OrderByDescending(
						param, query, parameters.SortBy);
				default:
					return query;
			}
		}

		/// <summary>
		/// Setups query paging for the <see cref="Get(GetOperationParameters)"/> operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="query">The query.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>The resulting query.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="query"/> is null.</exception>
		protected virtual IQueryable<TEntity> SetupGetQueryPaging(DbContext context,
			IQueryable<TEntity> query, QueryPagingParameters parameters)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(query, "query");

			if (parameters == null)
			{
				return query;
			}

			parameters.TotalCount = query.Count();

			return query.Skip(parameters.PageSize * parameters.PageNumber).Take(parameters.PageSize);
		}


		/// <summary>
		/// Loads the related entities.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entities">The entities.</param>
		/// <param name="relatedEntityInfo">Information about related entities.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/> is null.</exception>
		protected virtual void LoadRelatedEntities(DbContext context,
			TEntity[] entities, RelatedEntityInfo[] relatedEntityInfo)
		{
			Error.ArgumentNullException_IfNull(context, "context");

			if (entities == null || entities.Length == 0 || relatedEntityInfo == null
				|| relatedEntityInfo.Count() == 0)
			{
				return;
			}

			foreach (var info in relatedEntityInfo)
			{
				LoadRelatedEntities(context, entities, info);
			}

			if (ReflectionHelper.IsInherited(typeof(IEditable), typeof(TEntity)))
			{
				foreach (var entity in entities)
				{
					ForEachUsingEntity(entity, RecursionDirection.Descending, usingEntityContext =>
					{
						if (((IEditable)entity).IsUsed)
						{
							return;
						}

						//Skip nullified entities
						if (usingEntityContext.RelatedEntity == null)
						{
							return;
						}

						//Check only the first level entities
						var firstLevelInfo = usingEntityContext.RelatedEntityInfo.WithoutLast();
						if (!firstLevelInfo.IsEmpty() && !Entity().GetSubEntityInfo().Any(se => se.Includes(firstLevelInfo)))
						{
							return;
						}

						var collection = usingEntityContext.RelatedEntity as IEnumerable<object>;

						if (collection != null)
						{
							//It will set to true if there are items in the collection,
							//so the "else" statement will not be executed because of the check
							//at the top of the action
							((IEditable)entity).IsUsed = collection.Count() > 0;
						}
						else
						{
							//Here the entity is a referenced entity
							//It cannot be a collection item, because the "if" statement would
							//set the IsUsed property to true
							((IEditable)entity).IsUsed = true;
						}
					});
				}
			}
		}

		/// <summary>
		/// Creates the query filter interpreter.
		/// </summary>
		/// <returns>The query filter interpreter.</returns>
		protected virtual QueryFilterInterpreter CreateQueryFilterInterpreter()
		{
			return new QueryFilterInterpreter();
		}

		/// <summary>
		/// Setups the <see cref="GetTotalCount(OperationParameters)"/> operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="query">The query.</param>
		/// <returns>The resulting query.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="query"/> is null.</exception>
		protected virtual IQueryable<TEntity> SetupGetTotalCount(DbContext context, IQueryable<TEntity> query)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(query, "query");

			if (ReflectionHelper.IsInherited<ISoftDeletable, TEntity>())
			{
				var param = Expression.Parameter(typeof(TEntity));

				return (IQueryable<TEntity>)ExpressionBuilder.Where(query, ExpressionBuilder.Equals(param,
					ReflectionHelper.GetPropertyName<ISoftDeletable, bool>(e => e.IsDeleted), false), param);
			}

			return query;
		}

		/// <summary>
		/// Setups the <see cref="Add(AddOperationParameters{TEntity})"/> operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="parameters">The parameters.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/> or
		/// <paramref name="parameters"/> is null.</exception>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		protected virtual void SetupAdd(DbContext context, AddOperationParameters<TEntity> parameters)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			//Nullify using entities
			ForEachUsingEntity(parameters.Entity, RecursionDirection.Descending, usingEntityContext =>
			{
				RemoveUsingEntity(context, usingEntityContext);
			});

			ForEachUsedEntity(parameters.Entity, RecursionDirection.Descending, usedEntityContext =>
			{
				RemoveSecondLevelUsedEntities(usedEntityContext);
			});

			SynchronizeForeignKeysWithEntities(parameters.Entity);

			ForEachSubEntity(parameters.Entity, RecursionDirection.Ascending, subEntityContext =>
			{
				ProceedRelatedEntity(subEntityContext, null,
					(collection, item) =>
					{
						SetAdded(context, item, parameters);
					},
					(entity) =>
					{
						SetAdded(context, entity, parameters);
					}
				);
			});

			ForEachUsedEntity(parameters.Entity, RecursionDirection.Descending, usedEntityContext =>
			{
				ProceedRelatedEntity(usedEntityContext, null,
					(collection, item) =>
					{
						AddEntityToCollection(context, collection, item);
					},
					(usedEntity) =>
					{
						context.Entry(usedEntity).State = EntityState.Unchanged;
					}
				);
			});
		}

		/// <summary>
		/// Setups the <see cref="Update(UpdateOperationParameters{TEntity})"/> operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="existingEntity">The existing entity.</param>
		/// <param name="parameters">The parameters.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/> or
		/// <paramref name="parameters"/> is null.</exception>
		protected virtual void SetupUpdate(DbContext context, TEntity existingEntity,
			UpdateOperationParameters<TEntity> parameters)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			//Cleanup using entities
			ForEachUsingEntity(parameters.Entity, RecursionDirection.Descending, usingEntityContext =>
			{
				RemoveUsingEntity(context, usingEntityContext);
			});

			ForEachUsedEntity(parameters.Entity, RecursionDirection.Descending, usedEntityContext =>
			{
				RemoveSecondLevelUsedEntities(usedEntityContext);
			});

			SynchronizeForeignKeysWithEntities(parameters.Entity);

			ForEachUsedEntity(parameters.Entity, RecursionDirection.Descending, usedEntityContext =>
			{
				ProceedRelatedEntity(usedEntityContext,
					(collection) =>
					{
						var existingCollection = ReflectionHelper.GetPropertyValue<IEnumerable<object>>(
							existingEntity, usedEntityContext.RelatedEntityInfo.RelatedPropertyPath);

						//Find items in the existing collection,
						//which are absent in the current collection to remove them
						var itemsToRemove = existingCollection.Where(e => collection.All(
							c => !AreEntitiesEqual(context, e, c))).ToList();

						foreach (var item in itemsToRemove)
						{
							ReflectionHelper.RemoveAllReferences(item);

							RemoveEntityFromCollection(context, collection, item);
						}

						foreach (var item in collection.ToList())
						{
							//If the item is absent in the existing collection, then set it as added
							if (existingCollection.All(e => !AreEntitiesEqual(context, item, e)))
							{
								AddEntityToCollection(context, collection, item);
							}
							else
							{
								//If the item is exists in the existing collection, then set it as unchanged
								context.Entry(item).State = EntityState.Unchanged;
							}
						}
					},
					null,
					(usedEntity) =>
					{
						context.Entry(usedEntity).State = EntityState.Unchanged;
					}
				);
			});

			ForEachSubEntity(parameters.Entity, RecursionDirection.Descending, subEntityContext =>
			{
				ProceedRelatedEntity(subEntityContext,
					(collection) =>
					{
						var existingCollection = ReflectionHelper.GetPropertyValue<IEnumerable<object>>(
							existingEntity, subEntityContext.RelatedEntityInfo.RelatedPropertyPath);

						//Find items in the existing collection,
						//which are absent in the current collection to remove them
						var itemsToRemove = existingCollection.Where(e => collection.All(
							c => !AreEntitiesEqual(context, e, c))).ToList();

						foreach (var item in itemsToRemove)
						{
							ReflectionHelper.RemoveAllReferences(item);

							SetDeleted(context, item, parameters);
						}

						foreach (var item in collection.ToList())
						{
							//If the item is absent in the existing collection, then set it as added
							if (existingCollection.All(e => !AreEntitiesEqual(context, item, e)))
							{
								SetAdded(context, item, parameters);
							}
							else
							{
								//If the item is exists in the existing collection, then set it as modified
								SetModified(context, item, parameters);
							}
						}
					},
					null,
					(subEntity) =>
					{
						SetModified(context, subEntity, parameters);
					}
				);
			});
		}

		/// <summary>
		/// Setups the <see cref="Delete(DeleteOperationParameters)"/> operation.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entity">The entity to delete.</param>
		/// <param name="parameters">The delete operation parameters.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/> or
		/// <paramref name="parameters"/> is null.</exception>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		protected virtual void SetupDelete(DbContext context, TEntity entity, DeleteOperationParameters parameters)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			//Optimization: if there's no sub entities and used entities, then
			//there's no need to download the entity from the database to delete it.
			if (entity == null)
			{
				var fakeEntity = context.Set<TEntity>().Create();

				var pkNames = GetPrimaryKeyNames(context, typeof(TEntity));

				var pkValues = parameters.PrimaryKeys;

				for (int i = 0; i < parameters.PrimaryKeys.Length; i++)
				{
					ReflectionHelper.SetPropertyValue(fakeEntity, pkNames[i], pkValues[i]);
				}

				context.Entry(fakeEntity).State = EntityState.Unchanged;

				SetDeleted(context, fakeEntity, parameters);

				return;
			}

			//Cleanup using entities
			ForEachUsingEntity(entity, RecursionDirection.Descending, usingEntityContext =>
			{
				RemoveUsingEntity(context, usingEntityContext);
			});

			ForEachUsedEntity(entity, RecursionDirection.Descending, usedEntityContext =>
			{
				RemoveSecondLevelUsedEntities(usedEntityContext);
			});

			ForEachSubEntity(entity, RecursionDirection.Ascending, subEntityContext =>
			{
				ProceedRelatedEntity(subEntityContext, null,
					(collection, item) =>
					{
						SetDeleted(context, item, parameters);
					},
					(subEntity) =>
					{
						SetDeleted(context, subEntity, parameters);
					}
				);
			});

			ForEachUsedEntity(entity, RecursionDirection.Descending, usedEntityContext =>
			{
				ProceedRelatedEntity(usedEntityContext, null,
					(collection, item) =>
					{
						RemoveEntityFromCollection(context, collection, item);
					},
					(usedEntity) =>
					{
						context.Entry(usedEntity).State = EntityState.Unchanged;
					}
				);
			});
		}

		/// <summary>
		/// Gets the names of the primary keys.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entityType">The type of the entity.</param>
		/// <returns>The names of the primary keys.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="entityType"/> is null.</exception>
		protected string[] GetPrimaryKeyNames(DbContext context, Type entityType)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(entityType, "entityType");

			while (entityType != typeof(object))
			{
				//The problem is that entity set can be defined for a base type,
				//so we'll get exceptions until we find that base type
				//TODO: find better approach and reimplement
				try
				{
					var methodInfo = typeof(ObjectContext).GetMethod("CreateObjectSet",
						Type.EmptyTypes).MakeGenericMethod(entityType);

					dynamic objectSet = methodInfo.Invoke(((IObjectContextAdapter)context).ObjectContext, null);

					var keyMembers = ((EntityType)(objectSet.EntitySet.ElementType)).KeyMembers;

					return keyMembers.Select(k => (string)k.Name).ToArray();
				}
				catch
				{
				}

				entityType = entityType.BaseType;
			}

			return null;
		}

		/// <summary>
		/// Gets the filter by primary keys.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="primaryKeys">The primary keys.</param>
		/// <returns>The filter by primary keys</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="entityType"/> or <paramref name="primaryKeys"/> is null.</exception>
		protected QueryFilterExpression GetFilterByPrimaryKeys(DbContext context,
			Type entityType, object[] primaryKeys)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(entityType, "entityType");
			Error.ArgumentNullException_IfNull(primaryKeys, "primaryKeys");

			var pkNames = GetPrimaryKeyNames(context, entityType);

			QueryFilterExpression filter = new EqualsCondition()
			{
				Property = pkNames[0],
				Value = primaryKeys[0]
			};

			for (int i = 1; i < pkNames.Length; i++)
			{
				var nextFilter = new EqualsCondition()
				{
					Property = pkNames[i],
					Value = primaryKeys[i]
				};

				filter = new AndOperator()
				{
					LeftOperand = filter,
					RightOperand = nextFilter
				};
			}

			return filter;
		}

		/// <summary>
		/// Checks if two entities have the same values of primary keys.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entity1">The entity1.</param>
		/// <param name="entity2">The entity2.</param>
		/// <returns><c>true</c> if two entities have the same values of primary keys,
		/// otherwise <c>false</c>.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="entity1"/> or <paramref name="entity2"/> is null.</exception>
		protected bool AreEntitiesEqual(DbContext context, object entity1, object entity2)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(entity1, "entity1");
			Error.ArgumentNullException_IfNull(entity2, "entity2");

			if (entity1.GetType() != entity2.GetType())
			{
				return false;
			}

			foreach (var pkName in GetPrimaryKeyNames(context, entity1.GetType()))
			{
				if (!ReflectionHelper.GetPropertyValue<object>(entity1, pkName).Equals(
					ReflectionHelper.GetPropertyValue<object>(entity2, pkName)))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Creates the identical entity.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entity">The entity.</param>
		/// <returns>The identical entity.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="context"/>
		/// or <paramref name="entity"/> is null.</exception>
		protected object CreateIdenticalEntity(DbContext context, object entity)
		{
			Error.ArgumentNullException_IfNull(context, "context");
			Error.ArgumentNullException_IfNull(entity, "entity");

			var result = context.Set(entity.GetType()).Create();

			var pkNames = GetPrimaryKeyNames(context, entity.GetType());

			for (int i = 0; i < pkNames.Length; i++)
			{
				ReflectionHelper.SetPropertyValue(result, pkNames[i],
					ReflectionHelper.GetPropertyValue<object>(entity, pkNames[i]));
			}

			return result;
		}

		/// <summary>
		/// Synchronizes foreign keys with entities.
		/// </summary>
		/// <param name="rootEntity">The root entity.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="rootEntity"/> is null.</exception>
		protected void SynchronizeForeignKeysWithEntities(TEntity rootEntity)
		{
			Error.ArgumentNullException_IfNull(rootEntity, "rootEntity");

			//Adding and then setting entities to the unchanged state
			//synchronizes foreign keys with navigation properties
			using (var tempContext = CreateDbContext())
			{
				tempContext.Set(typeof(TEntity)).Add(rootEntity);

				Action<RelatedEntityActionContext<TEntity>> action =
					delegate(RelatedEntityActionContext<TEntity> relatedEntityContext)
					{
						if (relatedEntityContext.RelatedEntity == null)
						{
							return;
						}

						if (ReflectionHelper.IsInherited(typeof(IEnumerable<object>),
							relatedEntityContext.RelatedEntity.GetType()))
						{
							return;
						}

						tempContext.Entry(relatedEntityContext.RelatedEntity).State = EntityState.Unchanged;
					};

				ForEachSubEntity(rootEntity, RecursionDirection.Descending, action);

				ForEachUsedEntity(rootEntity, RecursionDirection.Descending, action);
			}
		}

		/// <summary>
		/// Creates the database context.
		/// </summary>
		/// <returns>The database context.</returns>
		protected abstract EFDbContext<TEntity> CreateDbContext();

		#endregion

		#region Private Methods

		/// <summary>
		/// Loads the related entities.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entities">The entities.</param>
		/// <param name="relatedEntityInfo">The related entity information.</param>
		private void LoadRelatedEntities(DbContext context, IEnumerable<object> entities,
			RelatedEntityInfo relatedEntityInfo)
		{
			foreach (var entity in entities)
			{
				var info = relatedEntityInfo.First();

				var nextInfo = relatedEntityInfo.WithoutFirst();

				var relatedEntityPropertyInfo = ReflectionHelper.GetPropertyInfo(entity.GetType(), info.RelatedPropertyPath);

				if (ReflectionHelper.IsInherited(typeof(IEnumerable<object>), relatedEntityPropertyInfo.PropertyType))
				{
					var collection = ReflectionHelper.GetPropertyValue<IEnumerable<object>>(entity, info.RelatedPropertyPath);

					if (collection == null)
					{
						var itemType = relatedEntityPropertyInfo.PropertyType.GetGenericArguments()[0];

						if (typeof(ISoftDeletable).IsAssignableFrom(itemType))
						{
							var param = Expression.Parameter(itemType);

							ExpressionBuilder.Where(context.Entry(entity).Collection(info.RelatedPropertyPath).Query(),
								ExpressionBuilder.Equals(param, ReflectionHelper.GetPropertyName<ISoftDeletable, bool>(
								e => e.IsDeleted), false), param).Load();
						}
						else
						{
							context.Entry(entity).Collection(info.RelatedPropertyPath).Query().Load();
						}
					}

					if (!nextInfo.IsEmpty())
					{
						collection = ReflectionHelper.GetPropertyValue<IEnumerable<object>>(entity, info.RelatedPropertyPath);

						//Collection was loaded, but if there is no entities, then do not continue
						if (collection != null)
						{
							LoadRelatedEntities(context, collection, nextInfo);
						}
					}
				}
				else
				{
					var member = ReflectionHelper.GetPropertyValue<object>(entity, info.RelatedPropertyPath);

					if (member == null)
					{
						context.Entry(entity).Reference(info.RelatedPropertyPath).Load();
					}

					if (!nextInfo.IsEmpty())
					{
						member = ReflectionHelper.GetPropertyValue<object>(entity, info.RelatedPropertyPath);

						//Member was loaded, but if there is no entities, then do not continue
						if (member == null)
						{
							return;
						}

						LoadRelatedEntities(context, new object[] { member }, nextInfo);
					}
				}
			}
		}

		/// <summary>
		/// Removes the using entity.
		/// </summary>
		/// <param name="context">The database context.</param>
		/// <param name="usingEntityContext">The using entity context.</param>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		private void RemoveUsingEntity(DbContext context, RelatedEntityActionContext<TEntity> usingEntityContext)
		{
			//Set a related entity reference to null to avoid its modification.
			usingEntityContext.RelatedEntityPropertyInfo
				.SetValue(usingEntityContext.RelatedEntityContainer, null, null);

			if (usingEntityContext.RelatedEntityKeyInfo != null)
			{
				DAError.UsingEntityKeyIsNotNullableException_IfUsingEntityKeyIsNotNullable(
					usingEntityContext.RelatedEntityContainer.GetType(), usingEntityContext.RelatedEntityKeyInfo);

				usingEntityContext.RelatedEntityKeyInfo.SetValue(
					usingEntityContext.RelatedEntityContainer, null, null);
			}
		}

		/// <summary>
		/// Removes the second level used entities.
		/// </summary>
		/// <param name="usedEntityContext">The used entity context.</param>
		private void RemoveSecondLevelUsedEntities(RelatedEntityActionContext<TEntity> usedEntityContext)
		{
			var usedEntity = usedEntityContext.RelatedEntity;

			//Skip nullified entities
			if (usedEntity == null || usedEntity is IEnumerable<object>)
			{
				return;
			}

			//Call the action only for the first level used entities
			//All the other should be nullified
			var firstLevelInfo = usedEntityContext.RelatedEntityInfo.WithoutLast();
			if (!firstLevelInfo.IsEmpty() && !Entity().GetSubEntityInfo().Any(se => se.Includes(firstLevelInfo)))
			{
				return;
			}

			//Here we're working with the first level used entity

			//Here we're working with a collection item or with a referenced entity

			ReflectionHelper.RemoveAllReferences(usedEntity);
		}

		/// <summary>
		/// Proceeds a related entity.
		/// </summary>
		/// <param name="relatedEntityContext">The related entity context.</param>
		/// <param name="collectionAction">The collection action.</param>
		/// <param name="collectionItemAction">The collection item action.</param>
		/// <param name="itemAction">The item action.</param>
		private void ProceedRelatedEntity(RelatedEntityActionContext<TEntity> relatedEntityContext,
			Action<IEnumerable<object>> collectionAction,
			Action<IEnumerable<object>, object> collectionItemAction, Action<object> itemAction)
		{
			var relatedEntity = relatedEntityContext.RelatedEntity;

			//Skip nullified entities
			if (relatedEntity == null)
			{
				return;
			}

			var collection = relatedEntity as IEnumerable<object>;

			if (collection != null)
			{
				if (collectionAction != null)
				{
					collectionAction(collection);
				}

				return;
			}

			if (relatedEntityContext.RelatedEntityPropertyInfo != null)
			{
				collection = relatedEntityContext.RelatedEntityPropertyInfo.GetValue(
					relatedEntityContext.RelatedEntityContainer, null) as IEnumerable<object>;

				//We're working with a collection item
				if (collection != null)
				{
					if (collectionItemAction != null)
					{
						collectionItemAction(collection, relatedEntity);
					}

					return;
				}
			}

			//We're working with a referenced entity
			if (itemAction != null)
			{
				itemAction(relatedEntity);
			}
		}

		/// <summary>
		/// Sets the entity as a fully added.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="parameters">The parameters.</param>
		private void SetAdded(DbContext context, object entity, OperationParameters parameters)
		{
			context.Entry(entity).State = EntityState.Added;

			var updateAuditableEntity = entity as IUpdateAuditable;
			if (updateAuditableEntity != null)
			{
				updateAuditableEntity.LastUpdatedById = parameters.OwnerId;
				updateAuditableEntity.LastUpdateOn = parameters.OperationDateTime;
			}

			var createAuditableEntity = entity as ICreateAuditable;
			if (createAuditableEntity != null)
			{
				createAuditableEntity.CreatedById = parameters.OwnerId;
				createAuditableEntity.CreatedOn = parameters.OperationDateTime;
			}
		}

		/// <summary>
		/// Sets the entity as fully deleted.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="parameters">The parameters.</param>
		private void SetDeleted(DbContext context, object entity, OperationParameters parameters)
		{
			var softDeleteableEntity = entity as ISoftDeletable;
			if (softDeleteableEntity == null)
			{
				context.Entry(entity).State = EntityState.Deleted;
				return;
			}

			context.Entry(entity).State = EntityState.Unchanged;

			softDeleteableEntity.IsDeleted = true;

			var updateAuditableEntity = entity as IUpdateAuditable;
			if (updateAuditableEntity != null)
			{
				updateAuditableEntity.LastUpdatedById = parameters.OwnerId;
				updateAuditableEntity.LastUpdateOn = parameters.OperationDateTime;
			}

			var createAuditableEntity = entity as ICreateAuditable;
			if (createAuditableEntity != null)
			{
				context.Entry(createAuditableEntity).Property(x => x.CreatedById).IsModified = false;
				context.Entry(createAuditableEntity).Property(x => x.CreatedOn).IsModified = false;
			}
		}

		/// <summary>
		/// Sets the entity as modified.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="parameters">The parameters.</param>
		private void SetModified(DbContext context, object entity, OperationParameters parameters)
		{
			context.Entry(entity).State = EntityState.Modified;

			var updateAuditableEntity = entity as IUpdateAuditable;
			if (updateAuditableEntity != null)
			{
				updateAuditableEntity.LastUpdatedById = parameters.OwnerId;
				updateAuditableEntity.LastUpdateOn = parameters.OperationDateTime;
			}

			var createAuditableEntity = entity as ICreateAuditable;
			if (createAuditableEntity != null)
			{
				context.Entry(createAuditableEntity).Property(e => e.CreatedById).IsModified = false;
				context.Entry(createAuditableEntity).Property(e => e.CreatedOn).IsModified = false;
			}
		}

		/// <summary>
		/// Adds the entity to collection.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="collection">The collection.</param>
		/// <param name="entity">The entity.</param>
		private void AddEntityToCollection(DbContext context, IEnumerable<object> collection, object entity)
		{
			context.Entry(entity).State = EntityState.Unchanged;

			collection.GetType().GetMethod("Remove").Invoke(collection, new object[] { entity });

			//This call registers removing, so after adding below the entity's relationship is marked as modified
			//Without this call, removing and then adding keeps the entity as unchanged,
			//so relationship is not saved into a database
			context.ChangeTracker.DetectChanges();

			collection.GetType().GetMethod("Add").Invoke(collection, new object[] { entity });
		}

		/// <summary>
		/// Removes the entity from collection.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="collection">The collection.</param>
		/// <param name="entity">The entity.</param>
		private void RemoveEntityFromCollection(DbContext context, IEnumerable<object> collection, object entity)
		{
			context.Entry(entity).State = EntityState.Unchanged;

			collection.GetType().GetMethod("Remove").Invoke(collection, new object[] { entity });
		}

		#endregion

		#region Repository Members

		/// <summary>
		/// Gets entities by the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <returns>Entities by the specified parameters.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="parameters"/> is null.</exception>
		protected override TEntity[] Get(GetOperationParameters parameters)
		{
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			using (var context = CreateDbContext())
			{
				IQueryable<TEntity> query = null;

				query = SetupGetQueryFiltering(context, context.Set<TEntity>(),
					parameters.QueryParameters != null ? parameters.QueryParameters.Filter : null);

				query = SetupGetQuerySorting(context, query, parameters.QueryParameters != null ?
					parameters.QueryParameters.SortingParameters : null);

				query = SetupGetQueryPaging(context, query, parameters.QueryParameters != null ?
					parameters.QueryParameters.PagingParameters : null);

				var entities = query.ToArray();

				if (parameters.LoadSubEntities)
				{
					LoadRelatedEntities(context, entities, Entity().GetSubEntityInfo());
				}

				if (parameters.LoadUsedEntities)
				{
					LoadRelatedEntities(context, entities, Entity().GetUsedEntityInfo());
				}

				if (parameters.LoadUsingEntities)
				{
					LoadRelatedEntities(context, entities, Entity().GetUsingEntityInfo());
				}

				return entities;
			}
		}

		/// <summary>
		/// Gets the total count of entities.
		/// </summary>
		/// <param name="parameters">The parameters to get entities.</param>
		/// <returns>The total count of entities.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="parameters"/> is null.</exception>
		protected override int GetTotalCount(OperationParameters parameters)
		{
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			using (var context = CreateDbContext())
			{
				return SetupGetTotalCount(context, context.Set<TEntity>()).Count();
			}
		}

		/// <summary>
		/// Adds an entity with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters to add an entity.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="parameters"/> is null.</exception>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
		/// A database command did not affect the expected number of rows. This usually indicates an optimistic
		/// concurrency violation; that is, a row has been changed in the database since it was queried.
		/// </exception>
		/// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
		/// The save was aborted because validation of entity property values failed.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
		/// on the same context instance.</exception>
		/// <exception cref="System.ObjectDisposedException">The context or connection have been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">
		/// Some error occurred attempting to process entities in the context either before or after sending commands
		/// to the database.
		/// </exception>
		protected override void Add(AddOperationParameters<TEntity> parameters)
		{
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			using (var context = CreateDbContext())
			{
				SetupAdd(context, parameters);

				context.SaveChanges();
			}
		}

		/// <summary>
		/// Updates an entity with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters to update an entity.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="parameters"/> is null.</exception>
		/// <exception cref="Nrepo.DataAccess.EntityNotFoundException">The entity to update was not found.</exception>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
		/// A database command did not affect the expected number of rows. This usually indicates an optimistic
		/// concurrency violation; that is, a row has been changed in the database since it was queried.
		/// </exception>
		/// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
		/// The save was aborted because validation of entity property values failed.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
		/// on the same context instance.</exception>
		/// <exception cref="System.ObjectDisposedException">The context or connection have been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">
		/// Some error occurred attempting to process entities in the context either before or after sending commands
		/// to the database.
		/// </exception>
		protected override void Update(UpdateOperationParameters<TEntity> parameters)
		{
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			using (var context = CreateDbContext())
			{
				var pkNames = GetPrimaryKeyNames(context, typeof(TEntity));

				var pkValues = pkNames.Select(k => ReflectionHelper.GetPropertyValue<object>(
					parameters.Entity, k)).ToArray();

				var entities = Get(new GetOperationParameters()
				{
					QueryParameters = new QueryParameters()
					{
						Filter = GetFilterByPrimaryKeys(context, typeof(TEntity), pkValues.ToArray())
					},
					LoadSubEntities = true,
					LoadUsedEntities = true,
					LoadUsingEntities = false
				});

				DAError.EntityNotFoundException_IfEntityNotFoundDuringUpdate<TEntity>(entities, pkValues);

				SetupUpdate(context, entities[0], parameters);

				context.SaveChanges();
			}
		}

		/// <summary>
		/// Deletes an entity with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters to delete an entity.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="parameters"/> is null.</exception>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
		/// A database command did not affect the expected number of rows. This usually indicates an optimistic
		/// concurrency violation; that is, a row has been changed in the database since it was queried.
		/// </exception>
		/// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
		/// The save was aborted because validation of entity property values failed.
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
		/// on the same context instance.</exception>
		/// <exception cref="System.ObjectDisposedException">The context or connection have been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">
		/// Some error occurred attempting to process entities in the context either before or after sending commands
		/// to the database.
		/// </exception>
		protected override void Delete(DeleteOperationParameters parameters)
		{
			Error.ArgumentNullException_IfNull(parameters, "parameters");

			using (var context = CreateDbContext())
			{
				//Optimization: if there's no sub entities and related entities,
				//then there's no need to download the entity from the database to delete it.
				if (Entity().GetSubEntityInfo().Length == 0
					&& Entity().GetUsedEntityInfo().Length == 0)
				{
					SetupDelete(context, null, parameters);

					context.SaveChanges();

					return;
				}

				var filter = GetFilterByPrimaryKeys(context, typeof(TEntity), parameters.PrimaryKeys);

				var entities = Get(new GetOperationParameters()
				{
					QueryParameters = new QueryParameters() { Filter = filter },
					LoadSubEntities = true,
					LoadUsedEntities = true,
					LoadUsingEntities = false
				});

				if (entities == null || entities.Length == 0)
				{
					return;
				}

				SetupDelete(context, entities[0], parameters);

				context.SaveChanges();
			}
		}

		#endregion
	}
}
