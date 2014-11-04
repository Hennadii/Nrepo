using System.Collections.Generic;

namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class Computer : SoftDeletableAuditableEntity
	{
		public Admin Admin
		{
			get;
			set;
		}

		public long AdminId
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
