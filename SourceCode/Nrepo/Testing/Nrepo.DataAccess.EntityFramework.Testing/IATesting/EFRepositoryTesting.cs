using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Common.IAEntities;

namespace Nrepo.DataAccess.EntityFramework.Testing.IATesting
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

			admin = new Admin() { Data = "data" };
			context.Admins.Add(admin);

			office1 = new Office() { Data = "data", Admin = admin };
			office2 = new Office() { Data = "data", Admin = admin };
			context.Offices.Add(office1);
			context.Offices.Add(office2);

			car1 = new Car() { Data = "data", Admin = admin };
			car2 = new Car() { Data = "data", Admin = admin };
			context.Cars.Add(car1);
			context.Cars.Add(car2);

			project1 = new Project() { Data = "data", Admin = admin };
			project2 = new Project() { Data = "data", Admin = admin };
			context.Projects.Add(project1);
			context.Projects.Add(project2);

			director = new Director() { Data = "data" };
			context.Directors.Add(director);

			directorTracker = new DirectorTracker()
			{
				Data = "data",
				Director = director,
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
			Assert.AreEqual("data", directors[0].Data);
			Assert.AreEqual(director.Id, directors[0].Id);

			Assert.AreEqual(1, directorTrackers.Length);
			Assert.AreEqual(directorTracker.Id, directorTrackers[0].Id);
			Assert.AreEqual("data", directorTrackers[0].Data);
			Assert.AreEqual(director.Id, directorTrackers[0].Director.Id);
		}

		private void AssertSecondLevelUsedEntities()
		{
			var context = new EFContext();

			var admins = context.Admins.ToArray();

			Assert.AreEqual(1, admins.Length);
			Assert.AreEqual("data", admins[0].Data);
		}

		private void AssertOffices()
		{
			var offices = new EFContext().Offices.ToArray();

			Assert.AreEqual(2, offices.Length);

			Assert.AreEqual(office1.Id, offices[0].Id);
			Assert.AreEqual("data", offices[0].Data);

			Assert.AreEqual(office2.Id, offices[1].Id);
			Assert.AreEqual("data", offices[1].Data);
		}

		private void AssertCars()
		{
			var cars = new EFContext().Cars.Include(ReflectionHelper
				.GetPropertyName<Car, Admin>(e => e.Admin)).ToArray();

			Assert.AreEqual(2, cars.Length);

			Assert.AreEqual(admin.Id, cars[0].Admin.Id);
			Assert.AreEqual(car1.Id, cars[0].Id);
			Assert.AreEqual("data", cars[0].Data);

			Assert.AreEqual(admin.Id, cars[1].Admin.Id);
			Assert.AreEqual(car2.Id, cars[1].Id);
			Assert.AreEqual("data", cars[1].Data);
		}

		private void AssertProjects(long? project1ManagerId, long? project2ManagerId)
		{
			var projects = new EFContext().Projects
				.Include(ReflectionHelper.GetPropertyName<Project, Admin>(e => e.Admin))
				.Include(ReflectionHelper.GetPropertyName<Project, Manager>(e => e.Manager))
				.ToArray();

			Assert.AreEqual(2, projects.Length);

			Assert.AreEqual(admin.Id, projects[0].Admin.Id);
			Assert.AreEqual(project1.Id, projects[0].Id);
			Assert.AreEqual("data", projects[0].Data);
			if (project1ManagerId != null)
			{
				Assert.AreEqual(project1ManagerId, projects[0].Manager.Id);
			}
			else
			{
				Assert.AreEqual(null, projects[0].Manager);
			}

			Assert.AreEqual(admin.Id, projects[1].Admin.Id);
			Assert.AreEqual(project2.Id, projects[1].Id);
			Assert.AreEqual("data", projects[1].Data);
			if (project2ManagerId != null)
			{
				Assert.AreEqual(project2ManagerId, projects[1].Manager.Id);
			}
			else
			{
				Assert.AreEqual(null, projects[1].Manager);
			}
		}

		private void AssertComputers(DateTime creationDate, DateTime updatingDate, string data, bool isDeleted)
		{
			var computers = new EFContext().Computers
				.Include(ReflectionHelper.GetPropertyName<Computer, Admin>(e => e.Admin))
				.ToArray();

			if (!isDeleted)
			{
				Assert.AreEqual(1, computers.Length);
			}
			else
			{
				Assert.AreEqual(0, computers.Length);
				return;
			}

			Assert.AreEqual(data, computers[0].Data);
			Assert.AreEqual(admin.Id, computers[0].Admin.Id);
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

			Assert.AreEqual(data, descriptions[0].Data);
		}

		private void AssertThing(Thing thing, DateTime creationDate,
			DateTime updatingDate, string data, long managerId)
		{
			Assert.AreEqual(data, thing.Data);
			Assert.AreEqual(admin.Id, thing.Admin.Id);
			Assert.AreEqual(managerId, thing.Manager.Id);
		}
	}
}
