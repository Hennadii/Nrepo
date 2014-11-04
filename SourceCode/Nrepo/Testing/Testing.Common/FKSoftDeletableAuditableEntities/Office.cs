using System.Collections.Generic;

namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class Office : SoftDeletableAuditableEntity
	{
		public Admin Admin
		{
			get;
			set;
		}

		public IList<Manager> Managers
		{
			get;
			set;
		}
	}
}
