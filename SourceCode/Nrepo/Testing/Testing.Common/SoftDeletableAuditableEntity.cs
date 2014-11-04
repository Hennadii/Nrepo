using System;
using Nrepo.Data;

namespace Testing.Common
{
	public abstract class SoftDeletableAuditableEntity : AuditableEntity,
		ISoftDeletable
	{
		public bool IsDeleted
		{
			get;
			set;
		}
	}
}
