using System.Collections.Generic;

namespace Testing.Common.FKAuditableEntities
{
	public class Director : AuditableEntity
	{
		public IList<DirectorTracker> DirectorTrackers
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
