using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Common.FKAuditableEntities;

namespace Nrepo.DataAccess.EntityFramework.Testing.FKAuditableTesting
{
	[TestClass]
	public partial class EFRepositoryTesting
	{
		DateTime initializingDate;
		Admin admin;
		Office office1, office2;
		Car car1, car2;
		Project project1, project2;
		Director director;
		DirectorTracker directorTracker;

		[ClassCleanup]
		public static void ClassCleanup()
		{
			EFContext.DeleteDatabase();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			EFContext.ClearDatabase();
		}

		[TestInitialize]
		public void TestInitialize()
		{
			initializingDate = DateTime.UtcNow;

			EFContext.ClearDatabase();

			var context = new EFContext();

			admin = new Admin() { Data = "data", CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			context.Admins.Add(admin);

			office1 = new Office() { Data = "data", Admin = admin, CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			office2 = new Office() { Data = "data", Admin = admin, CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			context.Offices.Add(office1);
			context.Offices.Add(office2);

			car1 = new Car() { Data = "data", Admin = admin, CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			car2 = new Car() { Data = "data", Admin = admin, CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			context.Cars.Add(car1);
			context.Cars.Add(car2);

			project1 = new Project() { Data = "data", Admin = admin, CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			project2 = new Project() { Data = "data", Admin = admin, CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			context.Projects.Add(project1);
			context.Projects.Add(project2);

			director = new Director() { Data = "data", CreatedOn = initializingDate, LastUpdateOn = initializingDate };
			context.Directors.Add(director);

			directorTracker = new DirectorTracker()
			{
				Data = "data",
				Director = director,
				CreatedOn = initializingDate,
				LastUpdateOn = initializingDate
			};
			context.DirectorTrackers.Add(directorTracker);

			context.SaveChanges();
		}

		private void AssertUsingEntities()
		{
			var context = new EFContext();

			var directors = context.Directors.ToArray();
			var directorTrackers = context.DirectorTrackers.ToArray();

			Assert.AreEqual(1, directors.Length);
			Assert.AreEqual(initializingDate, directors[0].CreatedOn);
			Assert.AreEqual(initializingDate, directors[0].LastUpdateOn);
			Assert.AreEqual("data", directors[0].Data);
			Assert.AreEqual(director.Id, directors[0].Id);

			Assert.AreEqual(1, directorTrackers.Length);
			Assert.AreEqual(directorTracker.Id, directorTrackers[0].Id);
			Assert.AreEqual(initializingDate, directorTrackers[0].CreatedOn);
			Assert.AreEqual(initializingDate, directorTrackers[0].LastUpdateOn);
			Assert.AreEqual("data", directorTrackers[0].Data);
			Assert.AreEqual(director.Id, directorTrackers[0].DirectorId);
		}

		private void AssertSecondLevelUsedEntities()
		{
			var context = new EFContext();

			var admins = context.Admins.ToArray();

			Assert.AreEqual(1, admins.Length);
			Assert.AreEqual(initializingDate, admins[0].CreatedOn);
			Assert.AreEqual(initializingDate, admins[0].LastUpdateOn);
			Assert.AreEqual("data", admins[0].Data);
		}

		private void AssertOffices()
		{
			var offices = new EFContext().Offices.ToArray();

			Assert.AreEqual(2, offices.Length);

			Assert.AreEqual(office1.Id, offices[0].Id);
			Assert.AreEqual(initializingDate, offices[0].CreatedOn);
			Assert.AreEqual(initializingDate, offices[0].LastUpdateOn);
			Assert.AreEqual("data", offices[0].Data);

			Assert.AreEqual(office2.Id, offices[1].Id);
			Assert.AreEqual(initializingDate, offices[1].CreatedOn);
			Assert.AreEqual(initializingDate, offices[1].LastUpdateOn);
			Assert.AreEqual("data", offices[1].Data);
		}

		private void AssertCars()
		{
			var cars = new EFContext().Cars.ToArray();

			Assert.AreEqual(2, cars.Length);

			Assert.AreEqual(admin.Id, cars[0].AdminId);
			Assert.AreEqual(car1.Id, cars[0].Id);
			Assert.AreEqual(initializingDate, cars[0].CreatedOn);
			Assert.AreEqual(initializingDate, cars[0].LastUpdateOn);
			Assert.AreEqual("data", cars[0].Data);

			Assert.AreEqual(admin.Id, cars[1].AdminId);
			Assert.AreEqual(car2.Id, cars[1].Id);
			Assert.AreEqual(initializingDate, cars[1].CreatedOn);
			Assert.AreEqual(initializingDate, cars[1].LastUpdateOn);
			Assert.AreEqual("data", cars[1].Data);
		}

		private void AssertProjects(long? project1ManagerId, long? project2ManagerId)
		{
			var projects = new EFContext().Projects.ToArray();

			Assert.AreEqual(2, projects.Length);

			Assert.AreEqual(admin.Id, projects[0].AdminId);
			Assert.AreEqual(project1.Id, projects[0].Id);
			Assert.AreEqual(initializingDate, projects[0].CreatedOn);
			Assert.AreEqual(initializingDate, projects[0].LastUpdateOn);
			Assert.AreEqual("data", projects[0].Data);
			Assert.AreEqual(project1ManagerId, projects[0].ManagerId);

			Assert.AreEqual(admin.Id, projects[1].AdminId);
			Assert.AreEqual(project2.Id, projects[1].Id);
			Assert.AreEqual(initializingDate, projects[1].CreatedOn);
			Assert.AreEqual(initializingDate, projects[1].LastUpdateOn);
			Assert.AreEqual("data", projects[1].Data);
			Assert.AreEqual(project2ManagerId, projects[1].ManagerId);
		}

		private void AssertComputers(DateTime creationDate, DateTime updatingDate, string data, bool isDeleted)
		{
			var computers = new EFContext().Computers.ToArray();

			if (!isDeleted)
			{
				Assert.AreEqual(1, computers.Length);
			}
			else
			{
				Assert.AreEqual(0, computers.Length);
				return;
			}

			Assert.AreEqual(creationDate, computers[0].CreatedOn);
			Assert.AreEqual(updatingDate, computers[0].LastUpdateOn);
			Assert.AreEqual(data, computers[0].Data);
			Assert.AreEqual(admin.Id, computers[0].AdminId);
		}

		private void AssertDescriptions(DateTime creationDate, DateTime updatingDate, string data, bool isDeleted)
		{
			var descriptions = new EFContext().Descriptions.ToArray();

			if (!isDeleted)
			{
				Assert.AreEqual(1, descriptions.Length);
			}
			else
			{
				Assert.AreEqual(0, descriptions.Length);
				return;
			}

			Assert.AreEqual(creationDate, descriptions[0].CreatedOn);
			Assert.AreEqual(updatingDate, descriptions[0].LastUpdateOn);
			Assert.AreEqual(data, descriptions[0].Data);
		}

		private void AssertThing(Thing thing, DateTime creationDate,
			DateTime updatingDate, string data, long managerId)
		{
			Assert.AreEqual(creationDate, thing.CreatedOn);
			Assert.AreEqual(updatingDate, thing.LastUpdateOn);
			Assert.AreEqual(data, thing.Data);
			Assert.AreEqual(admin.Id, thing.AdminId);
			Assert.AreEqual(managerId, thing.ManagerId);
		}
	}
}
