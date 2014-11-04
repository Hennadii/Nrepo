namespace Testing.Common.IAEntities
{
	public class Thing : EditableEntity
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
