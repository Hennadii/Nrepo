using System;
using Nrepo.DataAccess;

namespace Nrepo.DataAccess.Testing.Common
{
	public abstract class RepositoryTester<TEntity> : Repository<TEntity>
		where TEntity : class
	{
		public new RelatedEntityConfiguration<TEntity> Entity()
		{
			return base.Entity();
		}

		public new void ForEachSubEntity(TEntity rootEntity, RecursionDirection direction, Action<RelatedEntityActionContext<TEntity>> action)
		{
			base.ForEachSubEntity(rootEntity, direction, action);
		}

		public new void ForEachUsedEntity(TEntity rootEntity, RecursionDirection direction, Action<RelatedEntityActionContext<TEntity>> action)
		{
			base.ForEachUsedEntity(rootEntity, direction, action);
		}

		public new void ForEachUsingEntity(TEntity rootEntity, RecursionDirection direction, Action<RelatedEntityActionContext<TEntity>> action)
		{
			base.ForEachUsingEntity(rootEntity, direction, action);
		}
	}
}
