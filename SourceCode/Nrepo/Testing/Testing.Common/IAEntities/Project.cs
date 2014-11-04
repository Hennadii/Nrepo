namespace Testing.Common.IAEntities
{
	public class Project : EditableEntity
	{
		public Admin Admin
		{
			get;
			set;
		}

		public Manager Manager
		{
			get;
			set;
		}
	}
}
