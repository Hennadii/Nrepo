using System.Collections.Generic;

namespace Testing.Common.IAEntities
{
	public class Office : EditableEntity
	{
		public Admin Admin
		{
			get;
			set;
		}

		public IList<Manager> Managers
		{
			get;
			set;
		}
	}
}
