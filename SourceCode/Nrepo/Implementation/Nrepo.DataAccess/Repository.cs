using System;
using System.Linq;
using System.Collections.Generic;

namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the base class for all the repository implementations.
	/// </summary>
	/// <typeparam name="TEntity">The type of the repository entity.</typeparam>
	public abstract class Repository<TEntity>
		where TEntity : class
	{
		#region Fields

		private readonly IList<RelatedEntityInfo> subEntityInfo;

		private readonly IList<RelatedEntityInfo> usedEntityInfo;

		private readonly IList<RelatedEntityInfo> usingEntityInfo;

		private RelatedEntityConfiguration<TEntity> relatedEntityConfiguration;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the new instance of the <see cref="Repository{TEntity}"/> class.
		/// </summary>
		public Repository()
		{
			subEntityInfo = new List<RelatedEntityInfo>();

			usedEntityInfo = new List<RelatedEntityInfo>();

			usingEntityInfo = new List<RelatedEntityInfo>();
		}

		#endregion

		#region Operations

		/// <summary>
		/// Gets entities by specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters to get entities.</param>
		/// <returns>Entities by specified parameters.</returns>
		protected abstract TEntity[] Get(GetOperationParameters parameters);

		/// <summary>
		/// Gets a total count of entities.
		/// </summary>
		/// <param name="parameters">The parameters to get entities.</param>
		/// <returns>A total count of entities.</returns>
		protected abstract int GetTotalCount(OperationParameters parameters);

		/// <summary>
		/// Adds an entity with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters to add an entity.</param>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		protected abstract void Add(AddOperationParameters<TEntity> parameters);

		/// <summary>
		/// Updates an entity with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters to update an entity.</param>
		/// <exception cref="Nrepo.DataAccess.EntityNotFoundException">The entity for update was not found.</exception>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		protected abstract void Update(UpdateOperationParameters<TEntity> parameters);

		/// <summary>
		/// Deletes an entity with the specified parameters.
		/// </summary>
		/// <param name="parameters">The parameters to delete an entity.</param>
		/// <exception cref="Nrepo.DataAccess.UsingEntityKeyIsNotNullableException">The using entity foreign key is not nullable.</exception>
		protected abstract void Delete(DeleteOperationParameters parameters);

		#endregion

		#region Public Methods

		/// <summary>
		/// Initializes the repository before its first usage.
		/// </summary>
		public virtual void Initialize()
		{
			SetupEntityConfigurations();
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Gets the configuration object for related entities.
		/// </summary>
		/// <returns>The configuration object for related entities.</returns>
		protected RelatedEntityConfiguration<TEntity> Entity()
		{
			if (relatedEntityConfiguration == null)
			{
				relatedEntityConfiguration = new RelatedEntityConfiguration<TEntity>(
					subEntityInfo, usedEntityInfo, usingEntityInfo);
			}

			return relatedEntityConfiguration;
		}

		/// <summary>
		/// Iterates over all the sub entities which are not null and the root entity and runs the specified action.
		/// </summary>
		/// <param name="rootEntity">The root entity.</param>
		/// <param name="direction">A direction of the recursive iteration.</param>
		/// <param name="action">The action.</param>
		protected void ForEachSubEntity(TEntity rootEntity, RecursionDirection direction,
			Action<RelatedEntityActionContext<TEntity>> action)
		{
			if (rootEntity == null || action == null)
			{
				return;
			}

			if (direction == RecursionDirection.Ascending)
			{
				foreach (var info in Entity().GetSubEntityInfo())
				{
					ForEachRelatedEntity(rootEntity, rootEntity, info,
						new RelatedEntityInfo(), direction, action, null);
				}
			}

			action(new RelatedEntityActionContext<TEntity>()
			{
				RootEntity = rootEntity,
				RelatedEntity = rootEntity,
				RelatedEntityContainer = null,
				RelatedEntityInfo = null,
				RelatedEntityPropertyInfo = null,
				RelatedEntityKeyInfo = null
			});

			if (direction == RecursionDirection.Descending)
			{
				foreach (var info in Entity().GetSubEntityInfo())
				{
					ForEachRelatedEntity(rootEntity, rootEntity, info,
						new RelatedEntityInfo(), direction, action, null);
				}
			}
		}

		/// <summary>
		/// Iterates over all the used entities which are not null and runs the specified action.
		/// Iteration is executed in the recursive ascending direction.
		/// </summary>
		/// <param name="rootEntity">The root entity.</param>
		/// <param name="direction">A direction of the recursive iteration.</param>
		/// <param name="action">The action.</param>
		protected void ForEachUsedEntity(TEntity rootEntity, RecursionDirection direction,
			Action<RelatedEntityActionContext<TEntity>> action)
		{
			if (rootEntity == null || action == null)
			{
				return;
			}

			foreach (var info in Entity().GetUsedEntityInfo())
			{
				ForEachRelatedEntity(rootEntity, rootEntity, info, new RelatedEntityInfo(),
					direction, action, Entity().GetSubEntityInfo());
			}
		}

		/// <summary>
		/// Iterates over all the using entities which are not null and runs the specified action.
		/// Iteration is executed in the recursive ascending direction.
		/// </summary>
		/// <param name="rootEntity">The root entity.</param>
		/// <param name="direction">A direction of the recursive iteration.</param>
		/// <param name="action">The action.</param>
		protected void ForEachUsingEntity(TEntity rootEntity, RecursionDirection direction,
			Action<RelatedEntityActionContext<TEntity>> action)
		{
			if (rootEntity == null || action == null)
			{
				return;
			}

			foreach (var info in Entity().GetUsingEntityInfo())
			{
				ForEachRelatedEntity(rootEntity, rootEntity, info,
					new RelatedEntityInfo(), direction, action, Entity().GetSubEntityInfo());
			}
		}

		/// <summary>
		/// Sets up the entity configurations.
		/// </summary>
		protected abstract void SetupEntityConfigurations();

		#endregion

		#region Private Methods

		/// <summary>
		/// Recursively iterates over all the related entities and runs the specified action.
		/// Iteration is executed in the recursive ascending direction.
		/// </summary>
		/// <param name="rootEntity">The root entity.</param>
		/// <param name="relatedEntityContainer">The related entity container.</param>
		/// <param name="currentEntityInfo">The current entity information.</param>
		/// <param name="containerEntityInfo">The container's entity information.</param>
		/// <param name="direction">A direction of the recursive iteration.</param>
		/// <param name="action">The action.</param>
		/// <param name="subEntityInfo">The information about sub entities to skip.</param>
		private void ForEachRelatedEntity(TEntity rootEntity, object relatedEntityContainer,
			RelatedEntityInfo currentEntityInfo, RelatedEntityInfo containerEntityInfo,
			RecursionDirection direction, Action<RelatedEntityActionContext<TEntity>> action,
			RelatedEntityInfo[] subEntityInfo)
		{
			var info = currentEntityInfo.First();

			var absoluteInfo = RelatedEntityInfo.Combine(containerEntityInfo, info);

			//If we're working with used or using entities, then
			//skip the related entity if it is a part of a sub entity.
			//Do not call action for sub entities.
			var callAction = subEntityInfo == null
				|| !subEntityInfo.Any(se => se.Includes(absoluteInfo));

			var nextInfo = currentEntityInfo.WithoutFirst();

			if (ReflectionHelper.IsInherited<IEnumerable<object>>(relatedEntityContainer.GetType(), info.RelatedPropertyPath))
			{
				var collection = ReflectionHelper.GetPropertyValue<IEnumerable<object>>(
					relatedEntityContainer, info.RelatedPropertyPath);

				if (direction == RecursionDirection.Descending)
				{
					if (callAction)
					{
						action(new RelatedEntityActionContext<TEntity>()
						{
							RootEntity = rootEntity,
							RelatedEntity = collection,
							RelatedEntityContainer = relatedEntityContainer,
							RelatedEntityInfo = absoluteInfo,
							RelatedEntityPropertyInfo =
								ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedPropertyPath),
							RelatedEntityKeyInfo = !string.IsNullOrEmpty(info.RelatedKeyPath) ?
								ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedKeyPath) : null
						});
					}

					collection = ReflectionHelper.GetPropertyValue<IEnumerable<object>>(
						relatedEntityContainer, info.RelatedPropertyPath);

					//After the action above the collection can be null
					if (collection == null)
					{
						return;
					}
				}

				//Convert to list because the collection can be modified
				if (collection != null)
				{
					foreach (var item in collection.ToList())
					{
						if (direction == RecursionDirection.Ascending)
						{
							if (item != null && !nextInfo.IsEmpty())
							{
								ForEachRelatedEntity(rootEntity, item, nextInfo,
									absoluteInfo, direction, action, subEntityInfo);
							}
						}

						if (callAction)
						{
							action(new RelatedEntityActionContext<TEntity>()
							{
								RootEntity = rootEntity,
								RelatedEntity = item,
								RelatedEntityContainer = relatedEntityContainer,
								RelatedEntityInfo = absoluteInfo,
								RelatedEntityPropertyInfo =
									ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedPropertyPath),
								RelatedEntityKeyInfo = !string.IsNullOrEmpty(info.RelatedKeyPath) ?
									ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedKeyPath) : null
							});
						}

						if (direction == RecursionDirection.Descending)
						{
							if (item == null)
							{
								continue;
							}

							if (!nextInfo.IsEmpty())
							{
								ForEachRelatedEntity(rootEntity, item, nextInfo,
									absoluteInfo, direction, action, subEntityInfo);
							}
						}
					}
				}

				if (direction == RecursionDirection.Ascending)
				{
					if (callAction)
					{
						action(new RelatedEntityActionContext<TEntity>()
						{
							RootEntity = rootEntity,
							RelatedEntity = collection,
							RelatedEntityContainer = relatedEntityContainer,
							RelatedEntityInfo = absoluteInfo,
							RelatedEntityPropertyInfo =
								ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedPropertyPath),
							RelatedEntityKeyInfo = !string.IsNullOrEmpty(info.RelatedKeyPath) ?
								ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedKeyPath) : null
						});
					}
				}
			}
			else
			{
				var member = ReflectionHelper.GetPropertyValue<object>(relatedEntityContainer, info.RelatedPropertyPath);

				if (direction == RecursionDirection.Ascending)
				{
					if (member != null && !nextInfo.IsEmpty())
					{
						ForEachRelatedEntity(rootEntity, member, nextInfo,
							absoluteInfo, direction, action, subEntityInfo);
					}
				}

				if (callAction)
				{
					action(new RelatedEntityActionContext<TEntity>()
					{
						RootEntity = rootEntity,
						RelatedEntity = member,
						RelatedEntityContainer = relatedEntityContainer,
						RelatedEntityInfo = absoluteInfo,
						RelatedEntityPropertyInfo =
							ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedPropertyPath),
						RelatedEntityKeyInfo = !string.IsNullOrEmpty(info.RelatedKeyPath) ?
							ReflectionHelper.GetPropertyInfo(relatedEntityContainer.GetType(), info.RelatedKeyPath) : null
					});
				}

				if (direction == RecursionDirection.Descending)
				{
					member = ReflectionHelper.GetPropertyValue<object>(relatedEntityContainer, info.RelatedPropertyPath);

					if (member == null)
					{
						return;
					}

					if (!nextInfo.IsEmpty())
					{
						ForEachRelatedEntity(rootEntity, member, nextInfo,
							absoluteInfo, direction, action, subEntityInfo);
					}
				}
			}
		}

		#endregion
	}
}
