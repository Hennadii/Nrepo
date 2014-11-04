namespace Testing.Common.FKAuditableEntities
{
	public class Project : AuditableEntity
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

		public Manager Manager
		{
			get;
			set;
		}

		public long? ManagerId
		{
			get;
			set;
		}
	}
}
