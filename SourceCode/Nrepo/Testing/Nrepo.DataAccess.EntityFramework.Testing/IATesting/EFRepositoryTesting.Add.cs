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
		public void Add_WithRequiredSubEntitiesAndRequiredUsedEntities_Succeeds()
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

			//Act
			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = addingDate
			});

			//Assert
			var context = new EFContext();

			var managers = context.Managers
				.Include(ReflectionHelper.GetPropertyName<Manager, Car>(e => e.Car))
				.Include(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer))
				.Include(ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description))
				.Include(ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director))
				.Include(ReflectionHelper.GetPropertyName<Manager, Office>(e => e.Office))
				.ToArray();

			var computers = context.Computers.ToArray();

			AssertUsingEntities();

			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, addingDate, "data", false);

			//Main entity
			Assert.AreEqual(1, managers.Length);
			Assert.AreEqual("data", managers[0].Data);
			Assert.AreEqual(null, managers[0].Car);
			Assert.AreEqual(computers[0].Id, managers[0].Computer.Id);
			Assert.AreEqual(null, managers[0].Description);
			Assert.AreEqual(null, managers[0].Director);
			Assert.AreEqual(office1.Id, managers[0].Office.Id);

			//First level used entities
			AssertOffices();
		}

		[TestMethod]
		public void Add_WithAllSubEntitiesAndRequiredUsedEntities_Succeeds()
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

			//Act
			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = addingDate
			});

			//Assert
			var context = new EFContext();

			var managers = context.Managers
				.Include(ReflectionHelper.GetPropertyName<Manager, Car>(e => e.Car))
				.Include(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer))
				.Include(ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description))
				.Include(ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director))
				.Include(ReflectionHelper.GetPropertyName<Manager, Office>(e => e.Office))
				.ToArray();
			var computers = context.Computers.ToArray();
			var things = context.Things
				.Include(ReflectionHelper.GetPropertyName<Thing, Admin>(e => e.Admin))
				.Include(ReflectionHelper.GetPropertyName<Thing, Manager>(e => e.Manager))
				.ToArray();
			var descriptions = context.Descriptions.ToArray();

			AssertUsingEntities();

			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, addingDate, "data", false);

			AssertDescriptions(addingDate, addingDate, "data", false);

			Assert.AreEqual(2, things.Length);

			AssertThing(things[0], addingDate, addingDate, "data", managers[0].Id);

			AssertThing(things[1], addingDate, addingDate, "data", managers[0].Id);

			//Main entity
			Assert.AreEqual(1, managers.Length);
			Assert.AreEqual("data", managers[0].Data);
			Assert.AreEqual(null, managers[0].Car);
			Assert.AreEqual(computers[0].Id, managers[0].Computer.Id);
			Assert.AreEqual(descriptions[0].Id, managers[0].Description.Id);
			Assert.AreEqual(null, managers[0].Director);
			Assert.AreEqual(office1.Id, managers[0].Office.Id);

			//First level used entities
			AssertOffices();
		}

		[TestMethod]
		public void Add_WithAllSubEntitiesAndAllUsedEntities_Succeeds()
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

			//Act
			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = addingDate
			});

			//Assert
			var context = new EFContext();

			var managers = context.Managers
				.Include(ReflectionHelper.GetPropertyName<Manager, Car>(e => e.Car))
				.Include(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer))
				.Include(ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description))
				.Include(ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director))
				.Include(ReflectionHelper.GetPropertyName<Manager, Office>(e => e.Office))
				.ToArray();
			var computers = context.Computers.ToArray();
			var things = context.Things
				.Include(ReflectionHelper.GetPropertyName<Thing, Admin>(e => e.Admin))
				.Include(ReflectionHelper.GetPropertyName<Thing, Manager>(e => e.Manager))
				.ToArray();
			var descriptions = context.Descriptions.ToArray();

			AssertUsingEntities();
			AssertSecondLevelUsedEntities();

			//Sub entities
			AssertComputers(addingDate, addingDate, "data", false);

			AssertDescriptions(addingDate, addingDate, "data", false);

			Assert.AreEqual(2, things.Length);

			AssertThing(things[0], addingDate, addingDate, "data", managers[0].Id);

			AssertThing(things[1], addingDate, addingDate, "data", managers[0].Id);

			//Main entity
			Assert.AreEqual(1, managers.Length);
			Assert.AreEqual("data", managers[0].Data);
			Assert.AreEqual(car1.Id, managers[0].Car.Id);
			Assert.AreEqual(computers[0].Id, managers[0].Computer.Id);
			Assert.AreEqual(descriptions[0].Id, managers[0].Description.Id);
			Assert.AreEqual(null, managers[0].Director);
			Assert.AreEqual(office1.Id, managers[0].Office.Id);

			//First level used entities
			AssertOffices();

			AssertCars();

			AssertProjects(manager.Id, manager.Id);
		}
	}
}
