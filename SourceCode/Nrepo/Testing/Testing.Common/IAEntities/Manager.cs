using System.Collections.Generic;

namespace Testing.Common.IAEntities
{
	public class Manager : EditableEntity
	{
		public IList<ManagerTracker> ManagerTrackers
		{
			get;
			set;
		}

		public Director Director
		{
			get;
			set;
		}

		public IList<Thing> Things
		{
			get;
			set;
		}

		public Description Description
		{
			get;
			set;
		}

		public Computer Computer
		{
			get;
			set;
		}

		public Office Office
		{
			get;
			set;
		}

		public Car Car
		{
			get;
			set;
		}

		public IList<Project> Projects
		{
			get;
			set;
		}
	}
}
