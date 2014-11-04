using Nrepo.Data;

namespace Testing.Common
{
	public class EditableEntity : Entity, IEditable
	{
		public bool IsUsed
		{
			get;
			set;
		}
	}
}
