using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Nrepo.DataAccess.Testing.Common;
using Testing.Common;
using Testing.Common.FKSoftDeletableAuditableEntities;

namespace Nrepo.DataAccess.Testing
{
	[TestClass]
	public class RepositoryTesting
	{
		[TestMethod]
		public void Initialize_Always_CallsSetupEntityConfigurations()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;
			mock.Protected().Setup("SetupEntityConfigurations").Callback(() => { });

			//Act
			mock.Object.Initialize();

			//Assert
			mock.Protected().Verify("SetupEntityConfigurations", Times.Once());
		}

		[TestMethod]
		public void Entity_Always_ReturnsRelatedEntityConfiguration()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			var result = mock.Object.Entity();

			//Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void ForEachSubEntity_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Should be 2 infos. Optimization should remove the first configuration.
			mock.Object.Entity().HasSubEntities(e => e.Things);
			mock.Object.Entity().HasSubEntities(e => e.Things).WithEntity(e => e.Admin, e => e.AdminId);

			var manager = new Manager()
			{
				Things = new List<Thing>()
				{
					new Thing()
					{
						Admin = new Admin()
					},
					new Thing()
					{
						Admin = new Admin()
					},
					new Thing()
					{
						Admin = new Admin()
					}
				}
			};

			int thingsCount = 0;
			int adminsCount = 0;
			int managersCount = 0;

			Action<RelatedEntityActionContext<Manager>> action =
				delegate(RelatedEntityActionContext<Manager> context)
				{
					if (context.RelatedEntity is Manager)
					{
						managersCount++;
					}

					if (context.RelatedEntity is Admin)
					{
						adminsCount++;
					}

					if (context.RelatedEntity is Thing || context.RelatedEntity is IEnumerable<Thing>)
					{
						thingsCount++;
					}
				};

			//Act
			mock.Object.ForEachSubEntity(manager, RecursionDirection.Descending, action);

			//Assert
			Assert.AreEqual(1, managersCount);
			Assert.AreEqual(3, adminsCount);
			Assert.AreEqual(4, thingsCount);
		}

		[TestMethod]
		public void ForEachSubEntity_NullifiedSubEntities_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Should be 2 infos. Optimization should remove the first configuration.
			mock.Object.Entity().HasSubEntities(e => e.Things);
			mock.Object.Entity().HasSubEntities(e => e.Things).WithEntity(e => e.Admin, e => e.AdminId);

			var manager = new Manager()
			{
				Things = new List<Thing>()
				{
					new Thing()
					{
						Admin = new Admin()
					},
					null,
					new Thing()
					{
						Admin = new Admin()
					}
				}
			};

			int thingsCount = 0;
			int adminsCount = 0;
			int managersCount = 0;

			Action<RelatedEntityActionContext<Manager>> action =
				delegate(RelatedEntityActionContext<Manager> context)
				{
					if (context.RelatedEntity is Manager)
					{
						managersCount++;
					}

					if (context.RelatedEntity is Admin)
					{
						adminsCount++;
					}

					if (context.RelatedEntity is Thing || context.RelatedEntity is IEnumerable<Thing>
						|| (context.RelatedEntity == null && ReflectionHelper.IsInherited(
						typeof(IEnumerable<object>), context.RelatedEntityPropertyInfo.PropertyType)))
					{
						thingsCount++;
					}
				};

			//Act
			mock.Object.ForEachSubEntity(manager, RecursionDirection.Descending, action);

			//Assert
			Assert.AreEqual(1, managersCount);
			Assert.AreEqual(2, adminsCount);
			Assert.AreEqual(4, thingsCount);
		}

		[TestMethod]
		public void ForEachUsedEntity_NullifiedUsedEntities_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Should be 2 infos. Optimization should remove the first configuration.
			mock.Object.Entity().UsesEntities(e => e.Things);
			mock.Object.Entity().UsesEntities(e => e.Things).WithEntity(e => e.Admin, e => e.AdminId);

			var manager = new Manager()
			{
				Things = new List<Thing>()
				{
					new Thing()
					{
						Admin = new Admin()
					},
					null,
					new Thing()
					{
						Admin = new Admin()
					}
				}
			};

			int thingsCount = 0;
			int adminsCount = 0;
			int managersCount = 0;

			Action<RelatedEntityActionContext<Manager>> action =
				delegate(RelatedEntityActionContext<Manager> context)
				{
					if (context.RelatedEntity is Manager)
					{
						managersCount++;
					}

					if (context.RelatedEntity is Admin)
					{
						adminsCount++;
					}

					if (context.RelatedEntity is Thing || context.RelatedEntity is IEnumerable<Thing>
						|| (ReflectionHelper.IsInherited(typeof(IEnumerable<object>),
						context.RelatedEntityPropertyInfo.PropertyType) && context.RelatedEntity == null))
					{
						thingsCount++;
					}
				};

			//Act
			mock.Object.ForEachUsedEntity(manager, RecursionDirection.Descending, action);

			//Assert
			Assert.AreEqual(0, managersCount);
			Assert.AreEqual(2, adminsCount);
			Assert.AreEqual(4, thingsCount);
		}

		[TestMethod]
		public void ForEachUsedEntity_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Should be 2 infos. Optimization should remove the first configuration.
			mock.Object.Entity().UsesEntities(e => e.Things);
			mock.Object.Entity().UsesEntities(e => e.Things).WithEntity(e => e.Admin, e => e.AdminId);

			var manager = new Manager()
			{
				Things = new List<Thing>()
				{
					new Thing()
					{
						Admin = new Admin()
					},
					new Thing()
					{
						Admin = new Admin()
					},
					new Thing()
					{
						Admin = new Admin()
					}
				}
			};

			int thingsCount = 0;
			int adminsCount = 0;
			int managersCount = 0;

			Action<RelatedEntityActionContext<Manager>> action =
				delegate(RelatedEntityActionContext<Manager> context)
				{
					if (context.RelatedEntity is Manager)
					{
						managersCount++;
					}

					if (context.RelatedEntity is Admin)
					{
						adminsCount++;
					}

					if (context.RelatedEntity is Thing || context.RelatedEntity is IEnumerable<Thing>
						|| (ReflectionHelper.IsInherited(typeof(IEnumerable<object>),
						context.RelatedEntityPropertyInfo.PropertyType) && context.RelatedEntity == null))
					{
						thingsCount++;
					}
				};

			//Act
			mock.Object.ForEachUsedEntity(manager, RecursionDirection.Descending, action);

			//Assert
			Assert.AreEqual(0, managersCount);
			Assert.AreEqual(3, adminsCount);
			Assert.AreEqual(4, thingsCount);
		}

		[TestMethod]
		public void ForEachUsingEntity_NullifiedUsingEntities_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Should be 2 infos. Optimization should remove the first configuration.
			mock.Object.Entity().IsUsedByEntities(e => e.Things);
			mock.Object.Entity().IsUsedByEntities(e => e.Things).WithEntity(e => e.Admin, e => e.AdminId);

			var manager = new Manager()
			{
				Things = new List<Thing>()
				{
					new Thing()
					{
						Admin = new Admin()
					},
					null,
					new Thing()
					{
						Admin = new Admin()
					}
				}
			};

			int thingsCount = 0;
			int adminsCount = 0;
			int managersCount = 0;

			Action<RelatedEntityActionContext<Manager>> action =
				delegate(RelatedEntityActionContext<Manager> context)
				{
					if (context.RelatedEntity is Manager)
					{
						managersCount++;
					}

					if (context.RelatedEntity is Admin)
					{
						adminsCount++;
					}

					if (context.RelatedEntity is Thing || context.RelatedEntity is IEnumerable<Thing>
						|| (ReflectionHelper.IsInherited(typeof(IEnumerable<object>),
						context.RelatedEntityPropertyInfo.PropertyType) && context.RelatedEntity == null))
					{
						thingsCount++;
					}
				};

			//Act
			mock.Object.ForEachUsingEntity(manager, RecursionDirection.Descending, action);

			//Assert
			Assert.AreEqual(0, managersCount);
			Assert.AreEqual(2, adminsCount);
			Assert.AreEqual(4, thingsCount);
		}

		[TestMethod]
		public void ForEachUsingEntity_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Should be 2 infos. Optimization should remove the first configuration.
			mock.Object.Entity().IsUsedByEntities(e => e.Things);
			mock.Object.Entity().IsUsedByEntities(e => e.Things).WithEntity(e => e.Admin, e => e.AdminId);

			var manager = new Manager()
			{
				Things = new List<Thing>()
				{
					new Thing()
					{
						Admin = new Admin()
					},
					new Thing()
					{
						Admin = new Admin()
					},
					new Thing()
					{
						Admin = new Admin()
					}
				}
			};

			int thingsCount = 0;
			int adminsCount = 0;
			int managersCount = 0;

			Action<RelatedEntityActionContext<Manager>> action =
				delegate(RelatedEntityActionContext<Manager> context)
				{
					if (context.RelatedEntity is Manager)
					{
						managersCount++;
					}

					if (context.RelatedEntity is Admin)
					{
						adminsCount++;
					}

					if (context.RelatedEntity is Thing || context.RelatedEntity is IEnumerable<Thing>
						|| (ReflectionHelper.IsInherited(typeof(IEnumerable<object>),
						context.RelatedEntityPropertyInfo.PropertyType) && context.RelatedEntity == null))
					{
						thingsCount++;
					}
				};

			//Act
			mock.Object.ForEachUsingEntity(manager, RecursionDirection.Descending, action);

			//Assert
			Assert.AreEqual(0, managersCount);
			Assert.AreEqual(3, adminsCount);
			Assert.AreEqual(4, thingsCount);
		}
	}
}
