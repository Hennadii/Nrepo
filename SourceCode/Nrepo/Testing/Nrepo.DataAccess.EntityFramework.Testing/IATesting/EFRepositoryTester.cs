using System.Data.Entity;
using Testing.Common.IAEntities;

namespace Nrepo.DataAccess.EntityFramework.Testing.IATesting
{
	public class EFRepositoryTester : EFRepository<Manager>
	{
		protected override EFDbContext<Manager> CreateDbContext()
		{
			return new EFContext();
		}

		protected override void SetupEntityConfigurations()
		{
			Entity()
				.HasSubEntity<Computer, long>(e => e.Computer, null)
				.HasSubEntity<Description, long>(e => e.Description, null)
				.HasSubEntities(e => e.Things)

				.UsesEntity<Computer, long>(e => e.Computer, null).WithEntity<Admin, long>(e => e.Admin, null)
				.UsesEntities(e => e.Things).WithEntity<Admin, long>(e => e.Admin, null)
				.UsesEntity<Office, long>(e => e.Office, null)
				.UsesEntity<Car, long>(e => e.Car, null)
				.UsesEntities(e => e.Projects)

				.IsUsedByEntity<Director, long>(e => e.Director, null).WithEntities(e => e.DirectorTrackers)
				.IsUsedByEntities(e => e.ManagerTrackers);
		}

		public new void Add(AddOperationParameters<Manager> parameters)
		{
			base.Add(parameters);
		}

		public new void Delete(DeleteOperationParameters parameters)
		{
			base.Delete(parameters);
		}

		public new void Update(UpdateOperationParameters<Manager> parameters)
		{
			base.Update(parameters);
		}

		public new int GetTotalCount(OperationParameters parameters)
		{
			return base.GetTotalCount(parameters);
		}

		public new Manager[] Get(GetOperationParameters parameters)
		{
			return base.Get(parameters);
		}
	}
}
