using System.Collections.Generic;

namespace Testing.Common.FKAuditableEntities
{
	public class Description : AuditableEntity
	{
		public IList<Manager> Managers
		{
			get;
			set;
		}
	}
}
