using System;
using Nrepo.Data;

namespace Testing.Common
{
	public abstract class AuditableEntity : EditableEntity,
		ICreateAuditable, IUpdateAuditable
	{
		public long CreatedById
		{
			get;
			set;
		}

		public DateTime CreatedOn
		{
			get;
			set;
		}

		public long LastUpdatedById
		{
			get;
			set;
		}

		public DateTime LastUpdateOn
		{
			get;
			set;
		}
	}
}
