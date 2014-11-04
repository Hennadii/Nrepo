namespace Testing.Common.FKAuditableEntities
{
	public class ManagerTracker : AuditableEntity
	{
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
