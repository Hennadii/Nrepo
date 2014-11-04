using System.Collections.Generic;

namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class Director : SoftDeletableAuditableEntity
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
