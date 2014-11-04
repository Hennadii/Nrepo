using System.Collections.Generic;

namespace Testing.Common.IAEntities
{
	public class Director : EditableEntity
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
