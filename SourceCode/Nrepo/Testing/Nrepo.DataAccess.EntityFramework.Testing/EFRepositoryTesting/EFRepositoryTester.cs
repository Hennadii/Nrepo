using System;
using System.Data.Entity;
using System.Linq;

namespace Nrepo.DataAccess.EntityFramework.Testing.EFRepositoryTesting
{
	public class EFRepositoryTester : EFRepository<EFEntity>
	{
		public new void Add(AddOperationParameters<EFEntity> parameters)
		{
			base.Add(parameters);
		}

		public new QueryFilterInterpreter CreateQueryFilterInterpreter()
		{
			return base.CreateQueryFilterInterpreter();
		}

		public new void Delete(DeleteOperationParameters parameters)
		{
			base.Delete(parameters);
		}

		public new EFEntity[] Get(GetOperationParameters parameters)
		{
			return base.Get(parameters);
		}

		public new void LoadRelatedEntities(DbContext context, EFEntity[] entities, RelatedEntityInfo[] relatedEntityInfo)
		{
			base.LoadRelatedEntities(context, entities, relatedEntityInfo);
		}

		public new void SetupAdd(DbContext context, AddOperationParameters<EFEntity> parameters)
		{
			base.SetupAdd(context, parameters);
		}

		public new void SetupDelete(DbContext context, EFEntity entity, DeleteOperationParameters parameters)
		{
			base.SetupDelete(context, entity, parameters);
		}

		public new IQueryable<EFEntity> SetupGetQueryFiltering(DbContext context, IQueryable<EFEntity> query, QueryFilterExpression expression)
		{
			return base.SetupGetQueryFiltering(context, query, expression);
		}

		public new IQueryable<EFEntity> SetupGetQueryPaging(DbContext context, IQueryable<EFEntity> query, QueryPagingParameters parameters)
		{
			return base.SetupGetQueryPaging(context, query, parameters);
		}

		public new IQueryable<EFEntity> SetupGetTotalCount(DbContext context, IQueryable<EFEntity> query)
		{
			return base.SetupGetTotalCount(context, query);
		}

		public new IQueryable<EFEntity> SetupGetQuerySorting(DbContext context, IQueryable<EFEntity> query, QuerySortingParameters parameters)
		{
			return base.SetupGetQuerySorting(context, query, parameters);
		}

		public new void SetupUpdate(DbContext context, EFEntity existingEntity, UpdateOperationParameters<EFEntity> parameters)
		{
			base.SetupUpdate(context, existingEntity, parameters);
		}

		public new void Update(UpdateOperationParameters<EFEntity> parameters)
		{
			base.Update(parameters);
		}

		protected override void SetupEntityConfigurations()
		{
		}

		public new void SynchronizeForeignKeysWithEntities(EFEntity rootEntity)
		{
			base.SynchronizeForeignKeysWithEntities(rootEntity);
		}

		public new bool AreEntitiesEqual(DbContext context, object entity1, object entity2)
		{
			return base.AreEntitiesEqual(context, entity1, entity2);
		}

		public new QueryFilterExpression GetFilterByPrimaryKeys(DbContext context,
			Type entityType, object[] primaryKeys)
		{
			return base.GetFilterByPrimaryKeys(context, entityType, primaryKeys);
		}

		public new string[] GetPrimaryKeyNames(DbContext context, Type entityType)
		{
			return base.GetPrimaryKeyNames(context, entityType);
		}

		protected override EFDbContext<EFEntity> CreateDbContext()
		{
			return new EFContext();
		}
	}
}
