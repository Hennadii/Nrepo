//TODO: Find a solution to set relationship states as modified to update a database with new references

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Testing.Common.IAEntities;

//namespace Nrepo.DataAccess.EntityFramework.Testing.IATesting
//{
//	public partial class EFRepositoryTesting
//	{
//		[TestMethod]
//		public void Update_WithRequiredSubEntitiesAndRequiredUsedEntities_UpdateAllEntities_Succeeds()
//		{
//			//Arrange
//			var repository = new EFRepositoryTester();
//			repository.Initialize();

//			var addingDate = DateTime.UtcNow;

//			var manager = new Manager()
//			{
//				Director = director,
//				ManagerTrackers = new List<ManagerTracker>()
//				{
//					new ManagerTracker()
//				},
//				Office = office1,
//				Computer = new Computer() { Data = "data", Admin = admin },
//				Data = "data"
//			};

//			repository.Add(new AddOperationParameters<Manager>()
//			{
//				Entity = manager,
//				OperationDateTime = addingDate
//			});

//			//Act
//			var updatingDate = DateTime.UtcNow;

//			manager.Data = "data1";
//			manager.Computer.Data = "data1";
//			manager.Office = office2;

//			repository.Update(new UpdateOperationParameters<Manager>()
//			{
//				Entity = manager,
//				OperationDateTime = updatingDate
//			});

//			//Assert
//			var context = new EFContext();

//			var managers = context.Managers
//				.Include(ReflectionHelper.GetPropertyName<Manager, Car>(e => e.Car))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Office>(e => e.Office))
//				.ToArray();
//			var computers = context.Computers.ToArray();

//			AssertUsingEntities();
//			AssertSecondLevelUsedEntities();

//			//Sub entities
//			AssertComputers(addingDate, updatingDate, "data1", false);

//			//Main entity
//			Assert.AreEqual(1, managers.Length);
//			Assert.AreEqual("data1", managers[0].Data);
//			Assert.AreEqual(null, managers[0].Car);
//			Assert.AreEqual(computers[0].Id, managers[0].Computer.Id);
//			Assert.AreEqual(null, managers[0].Description);
//			Assert.AreEqual(null, managers[0].Director);
//			Assert.AreEqual(office2.Id, managers[0].Office.Id);

//			//First level used entities
//			AssertOffices();
//		}

//		[TestMethod]
//		public void Update_WithAllSubEntitiesAndRequiredUsedEntities_UpdateAllEntities_Succeeds()
//		{
//			//Arrange
//			var repository = new EFRepositoryTester();
//			repository.Initialize();

//			var addingDate = DateTime.UtcNow;

//			var manager = new Manager()
//			{
//				Director = director,
//				ManagerTrackers = new List<ManagerTracker>()
//				{
//					new ManagerTracker()
//				},
//				Office = office1,
//				Computer = new Computer() { Data = "data", Admin = admin },
//				Things = new List<Thing>()
//				{
//					new Thing()
//					{
//						Id = 1,
//						Admin = admin,
//						Data = "data"
//					},
//					new Thing()
//					{
//						Id = 2,
//						Admin = admin,
//						Data = "data"
//					}
//				},
//				Description = new Description()
//				{
//					Data = "data"
//				},
//				Data = "data"
//			};

//			repository.Add(new AddOperationParameters<Manager>()
//			{
//				Entity = manager,
//				OperationDateTime = addingDate
//			});

//			//Act
//			var updatingDate = DateTime.UtcNow;

//			manager.Data = "data1";
//			manager.Computer.Data = "data1";
//			manager.Description.Data = "data1";
//			manager.Office = office2;
//			manager.Things.RemoveAt(1);
//			manager.Things[0].Data = "data1";
//			manager.Things.Add(new Thing() { Admin = admin, Data = "data" });

//			repository.Update(new UpdateOperationParameters<Manager>()
//			{
//				Entity = manager,
//				OperationDateTime = updatingDate
//			});

//			//Assert
//			var context = new EFContext();

//			var managers = context.Managers
//				.Include(ReflectionHelper.GetPropertyName<Manager, Car>(e => e.Car))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Office>(e => e.Office))
//				.ToArray();
//			var computers = context.Computers.ToArray();
//			var things = context.Things.ToArray();
//			var descriptions = context.Descriptions.ToArray();

//			AssertUsingEntities();
//			AssertSecondLevelUsedEntities();

//			//Sub entities
//			AssertComputers(addingDate, updatingDate, "data1", false);

//			AssertDescriptions(addingDate, updatingDate, "data1", false);

//			Assert.AreEqual(2, things.Length);

//			AssertThing(things[0], addingDate, updatingDate, "data1", managers[0].Id);

//			AssertThing(things[1], updatingDate, updatingDate, "data", managers[0].Id);

//			//Main entity
//			Assert.AreEqual(1, managers.Length);
//			Assert.AreEqual("data1", managers[0].Data);
//			Assert.AreEqual(null, managers[0].Car);
//			Assert.AreEqual(computers[0].Id, managers[0].Computer.Id);
//			Assert.AreEqual(descriptions[0].Id, managers[0].Description.Id);
//			Assert.AreEqual(null, managers[0].Director);
//			Assert.AreEqual(office2.Id, managers[0].Office.Id);

//			//First level used entities
//			AssertOffices();
//		}

//		[TestMethod]
//		public void Update_WithAllSubEntitiesAndAllUsedEntities_UpdateAllEntities_Succeeds()
//		{
//			//Arrange
//			var repository = new EFRepositoryTester();
//			repository.Initialize();

//			var addingDate = DateTime.UtcNow;

//			var manager = new Manager()
//			{
//				Director = director,
//				ManagerTrackers = new List<ManagerTracker>()
//				{
//					new ManagerTracker()
//				},
//				Office = office1,
//				Computer = new Computer() { Data = "data", Admin = admin },
//				Things = new List<Thing>()
//				{
//					new Thing()
//					{
//						Id = 1,
//						Admin = admin,
//						Data = "data"
//					},
//					new Thing()
//					{
//						Id = 2,
//						Admin = admin,
//						Data = "data"
//					}
//				},
//				Description = new Description()
//				{
//					Data = "data"
//				},
//				Data = "data",
//				Car = car1,
//				Projects = new List<Project>()
//				{
//					project1
//				}
//			};

//			repository.Add(new AddOperationParameters<Manager>()
//			{
//				Entity = manager,
//				OperationDateTime = addingDate
//			});

//			//Act
//			var updatingDate = DateTime.UtcNow;

//			manager.Data = "data1";
//			manager.Computer.Data = "data1";
//			manager.Description.Data = "data1";
//			manager.Office = office2;
//			manager.Things.RemoveAt(1);
//			manager.Things[0].Data = "data1";
//			manager.Things.Add(new Thing() { Admin = admin, Data = "data" });
//			manager.Car = car2;
//			manager.Projects.Clear();
//			manager.Projects.Add(project2);

//			repository.Update(new UpdateOperationParameters<Manager>()
//			{
//				Entity = manager,
//				OperationDateTime = updatingDate
//			});

//			//Assert
//			var context = new EFContext();

//			var managers = context.Managers
//				.Include(ReflectionHelper.GetPropertyName<Manager, Car>(e => e.Car))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director))
//				.Include(ReflectionHelper.GetPropertyName<Manager, Office>(e => e.Office))
//				.ToArray();
//			var computers = context.Computers.ToArray();
//			var things = context.Things.ToArray();
//			var descriptions = context.Descriptions.ToArray();

//			AssertUsingEntities();
//			AssertSecondLevelUsedEntities();

//			//Sub entities
//			AssertComputers(addingDate, updatingDate, "data1", false);

//			AssertDescriptions(addingDate, updatingDate, "data1", false);

//			Assert.AreEqual(2, things.Length);

//			AssertThing(things[0], addingDate, updatingDate, "data1", managers[0].Id);

//			AssertThing(things[1], updatingDate, updatingDate, "data", managers[0].Id);

//			//Main entity
//			Assert.AreEqual(1, managers.Length);
//			Assert.AreEqual("data1", managers[0].Data);
//			Assert.AreEqual(car2.Id, managers[0].Car.Id);
//			Assert.AreEqual(computers[0].Id, managers[0].Computer.Id);
//			Assert.AreEqual(descriptions[0].Id, managers[0].Description.Id);
//			Assert.AreEqual(null, managers[0].Director);
//			Assert.AreEqual(office2.Id, managers[0].Office.Id);

//			//First level used entities
//			AssertOffices();

//			AssertCars();

//			AssertProjects(null, manager.Id);
//		}
//	}
//}
