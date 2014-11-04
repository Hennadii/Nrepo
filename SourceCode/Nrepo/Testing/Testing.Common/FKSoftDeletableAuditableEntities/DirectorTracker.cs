namespace Testing.Common.FKSoftDeletableAuditableEntities
{
	public class DirectorTracker : SoftDeletableAuditableEntity
	{
		public Director Director
		{
			get;
			set;
		}

		public long DirectorId
		{
			get;
			set;
		}
	}
}
