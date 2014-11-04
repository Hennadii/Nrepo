namespace Testing.Common.FKAuditableEntities
{
	public class DirectorTracker : AuditableEntity
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
