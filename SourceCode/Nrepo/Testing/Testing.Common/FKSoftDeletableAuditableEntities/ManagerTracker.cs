namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class ManagerTracker : SoftDeletableAuditableEntity
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
