using System.Collections.Generic;

namespace Testing.Common.FKAuditableEntities
{
	public class Manager : AuditableEntity
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

		public long? DirectorId
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

		public long? DescriptionId
		{
			get;
			set;
		}

		public Computer Computer
		{
			get;
			set;
		}

		public long ComputerId
		{
			get;
			set;
		}

		public Office Office
		{
			get;
			set;
		}

		public long OfficeId
		{
			get;
			set;
		}

		public Car Car
		{
			get;
			set;
		}

		public long? CarId
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
