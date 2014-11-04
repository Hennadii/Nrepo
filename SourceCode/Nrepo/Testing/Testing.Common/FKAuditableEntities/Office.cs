using System.Collections.Generic;

namespace Testing.Common.FKAuditableEntities
{
	public class Office : AuditableEntity
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
