using System.Collections.Generic;

namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class Description : SoftDeletableAuditableEntity
	{
		public IList<Manager> Managers
		{
			get;
			set;
		}
	}
}
