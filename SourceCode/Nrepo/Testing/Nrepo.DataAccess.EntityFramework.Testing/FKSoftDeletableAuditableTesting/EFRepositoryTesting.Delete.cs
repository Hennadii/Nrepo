using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Common.FKSoftDeletableAuditableEntities;

namespace Nrepo.DataAccess.EntityFramework.Testing.FKSoftDeletableAuditableTesting
{
	public partial class EFRepositoryTesting
	{
		[TestMethod]
		public void Delete_EntityNotExists_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();

			//Act
			//Assert
			repository.Initialize();

			repository.Delete(new DeleteOperationParameters()
			{
				PrimaryKeys = new object[] { 10000L }
			});
		}

		[TestMethod]
		public void Delete_WithRequiredSubEntitiesAndRequiredUsedEntities_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			var addingDate = DateTime.UtcNow;

			var manager = new Manager()
			{
				Director = director,
				ManagerTrackers = new List<ManagerTracker>()
				{
					new ManagerTracker()
				},
				Office = office1,
				Computer = new Computer() { Data = "data", Admin = admin },
				Data = "data"
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = addingDate
			});

			//Act
			director.Data = "data100";
			office1.Data = "data100";
			admin.Data = "data100";

			var deletingDate = DateTime.UtcNow;

			repository.Delete(new DeleteOperationParameters()
			{
				PrimaryKeys = new object[]
				{
					manager.Id
				},
				OperationDateTime = deletingDate
			});

			//Assert
			var context = new EFContext();

			var managers = context.Managers.ToArray();
			var computers = context.Computers.ToArray();

			AssertUsingEntities();
			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, deletingDate, "data", true);

			//Main entity
			Assert.AreEqual(1, managers.Length);
			Assert.AreEqual("data", managers[0].Data);
			Assert.AreEqual(addingDate, managers[0].CreatedOn);
			Assert.AreEqual(deletingDate, managers[0].LastUpdateOn);
			Assert.AreEqual(null, managers[0].CarId);
			Assert.AreEqual(computers[0].Id, managers[0].ComputerId);
			Assert.AreEqual(null, managers[0].DescriptionId);
			Assert.AreEqual(null, managers[0].DirectorId);
			Assert.AreEqual(true, managers[0].IsDeleted);
			Assert.AreEqual(office1.Id, managers[0].OfficeId);

			//First level used entities
			AssertOffices();
		}

		[TestMethod]
		public void Delete_WithAllSubEntitiesAndRequiredUsedEntities_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			var addingDate = DateTime.UtcNow;

			var manager = new Manager()
			{
				Director = director,
				ManagerTrackers = new List<ManagerTracker>()
				{
					new ManagerTracker()
				},
				Office = office1,
				Computer = new Computer() { Data = "data", Admin = admin },
				Things = new List<Thing>()
				{
					new Thing()
					{
						Id = 1,
						Admin = admin,
						Data = "data"
					},
					new Thing()
					{
						Id = 2,
						Admin = admin,
						Data = "data"
					}
				},
				Description = new Description()
				{
					Data = "data"
				},
				Data = "data"
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = addingDate
			});

			//Act
			director.Data = "data100";
			office1.Data = "data100";
			admin.Data = "data100";

			var deletingDate = DateTime.UtcNow;

			repository.Delete(new DeleteOperationParameters()
			{
				PrimaryKeys = new object[]
				{
					manager.Id
				},
				OperationDateTime = deletingDate
			});

			//Assert
			var context = new EFContext();

			var managers = context.Managers.ToArray();
			var computers = context.Computers.ToArray();
			var things = context.Things.ToArray();
			var descriptions = context.Descriptions.ToArray();

			AssertUsingEntities();
			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, deletingDate, "data", true);

			AssertDescriptions(addingDate, deletingDate, "data", true);

			Assert.AreEqual(2, things.Length);

			AssertThing(things[0], addingDate, deletingDate, "data", managers[0].Id, true);

			AssertThing(things[1], addingDate, deletingDate, "data", managers[0].Id, true);

			//Main entity
			Assert.AreEqual(1, managers.Length);
			Assert.AreEqual("data", managers[0].Data);
			Assert.AreEqual(addingDate, managers[0].CreatedOn);
			Assert.AreEqual(deletingDate, managers[0].LastUpdateOn);
			Assert.AreEqual(null, managers[0].CarId);
			Assert.AreEqual(computers[0].Id, managers[0].ComputerId);
			Assert.AreEqual(descriptions[0].Id, managers[0].DescriptionId);
			Assert.AreEqual(null, managers[0].DirectorId);
			Assert.AreEqual(true, managers[0].IsDeleted);
			Assert.AreEqual(office1.Id, managers[0].OfficeId);

			//First level used entities
			AssertOffices();
		}

		[TestMethod]
		public void Delete_WithAllSubEntitiesAndAllUsedEntities_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			var addingDate = DateTime.UtcNow;

			var manager = new Manager()
			{
				Director = director,
				ManagerTrackers = new List<ManagerTracker>()
				{
					new ManagerTracker()
				},
				Office = office1,
				Computer = new Computer() { Data = "data", Admin = admin },
				Things = new List<Thing>()
				{
					new Thing()
					{
						Id = 1,
						Admin = admin,
						Data = "data"
					},
					new Thing()
					{
						Id = 2,
						Admin = admin,
						Data = "data"
					}
				},
				Description = new Description()
				{
					Data = "data"
				},
				Data = "data",
				Car = car1,
				Projects = new List<Project>()
				{
					project1,
					project2
				}
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = addingDate
			});

			//Act
			director.Data = "data100";
			office1.Data = "data100";
			admin.Data = "data100";
			car1.Data = "data100";
			project1.Data = "data100";
			project2.Data = "data100";

			var deletingDate = DateTime.UtcNow;

			repository.Delete(new DeleteOperationParameters()
			{
				PrimaryKeys = new object[]
				{
					manager.Id
				},
				OperationDateTime = deletingDate
			});

			//Assert
			var context = new EFContext();

			var managers = context.Managers.ToArray();
			var computers = context.Computers.ToArray();
			var things = context.Things.ToArray();
			var descriptions = context.Descriptions.ToArray();

			AssertUsingEntities();
			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, deletingDate, "data", true);

			AssertDescriptions(addingDate, deletingDate, "data", true);

			Assert.AreEqual(2, things.Length);

			AssertThing(things[0], addingDate, deletingDate, "data", managers[0].Id, true);

			AssertThing(things[1], addingDate, deletingDate, "data", managers[0].Id, true);

			//Main entity
			Assert.AreEqual(1, managers.Length);
			Assert.AreEqual("data", managers[0].Data);
			Assert.AreEqual(addingDate, managers[0].CreatedOn);
			Assert.AreEqual(deletingDate, managers[0].LastUpdateOn);
			Assert.AreEqual(car1.Id, managers[0].CarId);
			Assert.AreEqual(computers[0].Id, managers[0].ComputerId);
			Assert.AreEqual(descriptions[0].Id, managers[0].DescriptionId);
			Assert.AreEqual(null, managers[0].DirectorId);
			Assert.AreEqual(true, managers[0].IsDeleted);
			Assert.AreEqual(office1.Id, managers[0].OfficeId);

			//First level used entities
			AssertOffices();

			AssertCars();

			AssertProjects(null, null);
		}
	}
}
