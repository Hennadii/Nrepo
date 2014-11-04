using System.Data.Entity;
using Testing.Common.FKSoftDeletableAuditableEntities;

namespace Nrepo.DataAccess.EntityFramework.Testing.FKSoftDeletableAuditableTesting
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
				.HasSubEntity(e => e.Computer, e => e.ComputerId)
				.HasSubEntity(e => e.Description, e => e.DescriptionId)
				.HasSubEntities(e => e.Things)

				.UsesEntity(e => e.Computer, e => e.ComputerId).WithEntity(e => e.Admin, e => e.AdminId)
				.UsesEntities(e => e.Things).WithEntity(e => e.Admin, e => e.AdminId)
				.UsesEntity(e => e.Office, e => e.OfficeId)
				.UsesEntity(e => e.Car, e => e.CarId)
				.UsesEntities(e => e.Projects)

				.IsUsedByEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.DirectorTrackers)
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
