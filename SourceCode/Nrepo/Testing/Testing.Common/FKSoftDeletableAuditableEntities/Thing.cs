namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class Thing : SoftDeletableAuditableEntity
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

		public long ManagerId
		{
			get;
			set;
		}
	}
}
