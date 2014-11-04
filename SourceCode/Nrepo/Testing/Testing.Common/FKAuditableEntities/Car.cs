﻿using System.Collections.Generic;

namespace Testing.Common.FKAuditableEntities
{
	public class Car : AuditableEntity
	{
		public Admin Admin
		{
			get;
			set;
		}

		public long AdminId
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
