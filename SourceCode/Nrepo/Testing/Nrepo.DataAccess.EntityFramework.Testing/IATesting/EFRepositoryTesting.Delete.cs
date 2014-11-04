using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Common.IAEntities;

namespace Nrepo.DataAccess.EntityFramework.Testing.IATesting
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
			Assert.AreEqual(0, managers.Length);

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
			var things = context.Things.ToArray();

			AssertUsingEntities();
			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, deletingDate, "data", true);

			AssertDescriptions(addingDate, deletingDate, "data", true);

			Assert.AreEqual(0, things.Length);

			//Main entity
			Assert.AreEqual(0, managers.Length);

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
			var things = context.Things.ToArray();

			AssertUsingEntities();
			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, deletingDate, "data", true);

			AssertDescriptions(addingDate, deletingDate, "data", true);

			Assert.AreEqual(0, things.Length);

			//Main entity
			Assert.AreEqual(0, managers.Length);

			//First level used entities
			AssertOffices();

			AssertCars();

			AssertProjects(null, null);
		}
	}
}
