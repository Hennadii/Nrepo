namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class Project : SoftDeletableAuditableEntity
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
