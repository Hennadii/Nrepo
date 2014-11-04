using System.Data.Entity;

namespace Nrepo.DataAccess.EntityFramework.Testing.EFRepositoryTesting
{
	public class EFContext : EFDbContext<EFEntity>
	{
		private static readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB_Nrepo_UnitTesting;Integrated Security=True;MultipleActiveResultSets=True";

		static EFContext()
		{
			Database.SetInitializer<EFContext>(new DropCreateDatabaseAlways<EFContext>());
		}

		public EFContext()
			: base(connectionString)
		{
		}

		public DbSet<EFEntity> EFEntities
		{
			get;
			set;
		}

		public DbSet<EFEntityChild> EFEntityChildren
		{
			get;
			set;
		}

		public static void DeleteDatabase()
		{
			if (!Database.Exists(connectionString))
			{
				return;
			}

			Database.Delete(connectionString);
		}
	}
}
