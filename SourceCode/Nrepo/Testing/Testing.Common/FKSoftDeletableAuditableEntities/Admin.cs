using System.Collections.Generic;

namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class Admin : SoftDeletableAuditableEntity
	{
		public IList<Thing> Things
		{
			get;
			set;
		}

		public IList<Computer> Computers
		{
			get;
			set;
		}

		public IList<Office> Offices
		{
			get;
			set;
		}

		public IList<Car> Cars
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
