using System.Collections.Generic;

namespace Nrepo.DataAccess.EntityFramework.Testing.EFRepositoryTesting
{
	public class EFEntity
	{
		public long Id
		{
			get;
			set;
		}

		public IList<EFEntityChild> EFEntityChildren
		{
			get;
			set;
		}
	}
}
