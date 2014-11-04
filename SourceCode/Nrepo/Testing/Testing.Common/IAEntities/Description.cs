using System.Collections.Generic;

namespace Testing.Common.IAEntities
{
	public class Description : EditableEntity
	{
		public IList<Manager> Managers
		{
			get;
			set;
		}
	}
}
