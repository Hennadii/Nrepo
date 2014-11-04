using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Common.FKAuditableEntities;

namespace Nrepo.DataAccess.EntityFramework.Testing.FKAuditableTesting
{
	public partial class EFRepositoryTesting
	{
		private void Add10Managers()
		{
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

			for (int i = 0; i < 10; i++)
			{
				manager.Data = "data" + i;

				repository.Add(new AddOperationParameters<Manager>()
				{
					Entity = manager,
					OwnerId = i / 3L,
					OperationDateTime = addingDate
				});
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetTotalCount_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			//Act
			//Assert
			var result = repository.GetTotalCount(null);
		}

		[TestMethod]
		public void GetTotalCount_NoEntities_ReturnsZero()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			//Act
			var result = repository.GetTotalCount(new OperationParameters());

			//Assert
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void GetTotalCount_EntitiesExist_ReturnsEntitiesCount()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			var addingDate = DateTime.UtcNow;

			var manager1 = new Manager()
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
					project1
				}
			};

			var manager2 = new Manager()
			{
				Director = director,
				ManagerTrackers = new List<ManagerTracker>()
				{
					new ManagerTracker()
				},
				Office = office2,
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
				Car = car2,
				Projects = new List<Project>()
				{
					project2
				}
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager1,
				OperationDateTime = addingDate
			});

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager2,
				OperationDateTime = addingDate
			});

			repository.Delete(new DeleteOperationParameters()
			{
				OperationDateTime = DateTime.UtcNow,
				PrimaryKeys = new object[] { manager2.Id }
			});

			//Act
			var result = repository.GetTotalCount(new OperationParameters());

			//Assert
			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void Get_ByPrimaryKeysAndNoEntities_ReturnsEmptyArray()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					Filter = new EqualsCondition()
					{
						Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.Id),
						Value = 123L
					}
				}
			});

			//Assert
			Assert.IsNotNull(entities);
			Assert.AreEqual(0, entities.Length);
		}

		[TestMethod]
		public void Get_ByFilter_ReturnsEntities()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			Add10Managers();

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					Filter = new OrOperator()
					{
						LeftOperand = new ContainsCondition()
						{
							Property = ReflectionHelper.GetPropertyName<Manager, string>(e => e.Data),
							Value = "9"
						},
						RightOperand = new OrOperator()
						{
							LeftOperand = new EqualsCondition()
							{
								Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
								Value = 0L
							},
							RightOperand = new EqualsCondition()
							{
								Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
								Value = 1L
							}
						}
					}
				}
			});

			//Assert
			Assert.AreEqual(7, entities.Length);
			foreach (var entity in entities)
			{
				Assert.IsTrue(entity.CreatedById == 0
					|| entity.CreatedById == 1
					|| entity.Data == "data9");
			}
		}

		[TestMethod]
		public void Get_ByFilterWhenDeletedEntitiesExist_ReturnsNotDeletedEntities()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			Add10Managers();

			var entitiesToDelete = repository.Get(new GetOperationParameters());

			repository.Delete(new DeleteOperationParameters()
			{
				PrimaryKeys = new object[] { entitiesToDelete[0].Id }
			});

			repository.Delete(new DeleteOperationParameters()
			{
				PrimaryKeys = new object[] { entitiesToDelete[1].Id }
			});

			repository.Delete(new DeleteOperationParameters()
			{
				PrimaryKeys = new object[] { entitiesToDelete[2].Id }
			});

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					Filter = new OrOperator()
					{
						LeftOperand = new ContainsCondition()
						{
							Property = ReflectionHelper.GetPropertyName<Manager, string>(e => e.Data),
							Value = "9"
						},
						RightOperand = new OrOperator()
						{
							LeftOperand = new EqualsCondition()
							{
								Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
								Value = 0L
							},
							RightOperand = new EqualsCondition()
							{
								Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
								Value = 1L
							}
						}
					}
				}
			});

			//Assert
			Assert.AreEqual(4, entities.Length);
			foreach (var entity in entities)
			{
				Assert.IsTrue(entity.CreatedById == 1
					|| entity.Data == "data9");
			}
		}

		[TestMethod]
		public void Get_ByFilterForUsedEntity_ReturnsEntities()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			Add10Managers();

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					Filter = new EqualsCondition()
					{
						Property = ReflectionHelper.GetPropertyName<Manager, string>(e => e.Car.Data),
						Value = "data"
					}
				}
			});

			//Assert
			Assert.AreEqual(10, entities.Length);
			for (int i = 1; i < 10; i++)
			{
				Assert.IsTrue(entities[i].Id - entities[i - 1].Id == 1);
			}
		}

		[TestMethod]
		public void Get_WithSorting_ReturnsSortedEntities()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			Add10Managers();

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					Filter = new OrOperator()
					{
						LeftOperand = new ContainsCondition()
						{
							Property = ReflectionHelper.GetPropertyName<Manager, string>(e => e.Data),
							Value = "9"
						},
						RightOperand = new OrOperator()
						{
							LeftOperand = new EqualsCondition()
							{
								Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
								Value = 0L
							},
							RightOperand = new EqualsCondition()
							{
								Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
								Value = 1L
							}
						}
					},
					SortingParameters = new QuerySortingParameters()
					{
						SortBy = ReflectionHelper.GetPropertyName<Manager, string>(e => e.Data),
						SortingType = SortingType.Descending
					}
				}
			});

			//Assert
			Assert.AreEqual(7, entities.Length);
			foreach (var entity in entities)
			{
				Assert.IsTrue(entity.CreatedById == 0
					|| entity.CreatedById == 1
					|| entity.Data == "data9");
			}

			Assert.AreEqual("data9", entities[0].Data);
			Assert.AreEqual("data5", entities[1].Data);
			Assert.AreEqual("data4", entities[2].Data);
			Assert.AreEqual("data3", entities[3].Data);
			Assert.AreEqual("data2", entities[4].Data);
			Assert.AreEqual("data1", entities[5].Data);
			Assert.AreEqual("data0", entities[6].Data);
		}

		[TestMethod]
		public void Get_WhenPageNumberIs0AndPageSizeIs3_ReturnsFirstThreeEntities()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			Add10Managers();

			//Act
			var getParameters = new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					PagingParameters = new QueryPagingParameters()
					{
						PageNumber = 0,
						PageSize = 3,
					}
				}
			};

			var entities = repository.Get(getParameters);

			//Assert
			Assert.AreEqual(3, entities.Length);
			Assert.AreEqual(10, getParameters.QueryParameters.PagingParameters.TotalCount);

			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(0, entities[i].CreatedById);
			}
		}

		[TestMethod]
		public void Get_WhenPageNumberIs4AndPageSizeIs3_ReturnsLastEntity()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			Add10Managers();

			//Act
			var getParameters = new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					PagingParameters = new QueryPagingParameters()
					{
						PageNumber = 3,
						PageSize = 3,
					}
				}
			};

			var entities = repository.Get(getParameters);

			//Assert
			Assert.AreEqual(1, entities.Length);
			Assert.AreEqual(10, getParameters.QueryParameters.PagingParameters.TotalCount);
			Assert.AreEqual(3, entities[0].CreatedById);
		}

		[TestMethod]
		public void Get_WhenFilteredAndSortedAndPaged_ReturnsEntities()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

			Add10Managers();

			//Act
			var getParameters = new GetOperationParameters()
			{
				QueryParameters = new QueryParameters()
				{
					Filter = new OrOperator()
					{
						LeftOperand = new EqualsCondition()
						{
							Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
							Value = 2L
						},
						RightOperand = new EqualsCondition()
						{
							Property = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
							Value = 3L
						}
					},
					SortingParameters = new QuerySortingParameters()
					{
						SortBy = ReflectionHelper.GetPropertyName<Manager, long>(e => e.CreatedById),
						SortingType = SortingType.Descending
					},
					PagingParameters = new QueryPagingParameters()
					{
						PageNumber = 0,
						PageSize = 3,
					}
				}
			};

			var entities = repository.Get(getParameters);

			//Assert
			Assert.AreEqual(3, entities.Length);
			Assert.AreEqual(4, getParameters.QueryParameters.PagingParameters.TotalCount);
			Assert.AreEqual(3, entities[0].CreatedById);
			Assert.AreEqual(2, entities[1].CreatedById);
			Assert.AreEqual(2, entities[2].CreatedById);
		}

		[TestMethod]
		public void Get_WithoutUsingEntities_IsUsedIsFalse()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

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
					//project2
				}
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = initializingDate
			});

			//Attach using entities
			var context = new EFContext();

			context.Directors.Attach(director);
			context.Managers.Attach(manager);
			director.Managers = new List<Manager>();
			director.Managers.Add(manager);

			context.ManagerTrackers.Add(new ManagerTracker()
			{
				ManagerId = manager.Id,
				Data = "data",
				CreatedOn = initializingDate,
				LastUpdateOn = initializingDate
			});

			context.SaveChanges();

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				LoadSubEntities = true,
				LoadUsedEntities = true,
				LoadUsingEntities = false,
			});

			//Assert
			Assert.AreEqual(false, entities[0].IsUsed);
		}

		[TestMethod]
		public void Get_WithUsingEntities_IsUsedIsTrue()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

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
					//project2
				}
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = initializingDate
			});

			//Attach using entities
			var context = new EFContext();

			context.Directors.Attach(director);
			context.Managers.Attach(manager);
			director.Managers = new List<Manager>();
			director.Managers.Add(manager);

			context.ManagerTrackers.Add(new ManagerTracker()
			{
				ManagerId = manager.Id,
				Data = "data",
				CreatedOn = initializingDate,
				LastUpdateOn = initializingDate
			});

			context.SaveChanges();

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				LoadSubEntities = true,
				LoadUsedEntities = true,
				LoadUsingEntities = true,
			});

			//Assert
			Assert.AreEqual(true, entities[0].IsUsed);
		}

		[TestMethod]
		public void Get_WhenNoUsingEntities_IsUsedIsFalse()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

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
					//project2
				}
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = initializingDate
			});

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				LoadSubEntities = true,
				LoadUsedEntities = true,
				LoadUsingEntities = true,
			});

			//Assert
			Assert.AreEqual(false, entities[0].IsUsed);
		}

		[TestMethod]
		public void Get_WithAllRelatedEntities_ReturnsEntities()
		{
			//Arrange
			var repository = new EFRepositoryTester();
			repository.Initialize();

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
					//project2
				}
			};

			repository.Add(new AddOperationParameters<Manager>()
			{
				Entity = manager,
				OperationDateTime = initializingDate
			});

			//Attach using entities
			var context = new EFContext();

			context.Directors.Attach(director);
			context.Managers.Attach(manager);
			director.Managers = new List<Manager>();
			director.Managers.Add(manager);

			context.ManagerTrackers.Add(new ManagerTracker()
			{
				ManagerId = manager.Id,
				Data = "data",
				CreatedOn = initializingDate,
				LastUpdateOn = initializingDate
			});

			context.SaveChanges();

			//Act
			var entities = repository.Get(new GetOperationParameters()
			{
				LoadSubEntities = true,
				LoadUsedEntities = true,
				LoadUsingEntities = true,
			});

			//Assert
			Assert.AreEqual(true, entities[0].IsUsed);

			AssertUsingEntities();

			AssertSecondLevelUsedEntities();

			//Sub entities
			Assert.AreEqual(1, entities.Length);
			Assert.AreEqual(initializingDate, entities[0].Computer.CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].Computer.LastUpdateOn);
			Assert.AreEqual("data", entities[0].Computer.Data);
			Assert.AreEqual(admin.Id, entities[0].Computer.AdminId);

			Assert.AreEqual(initializingDate, entities[0].Description.CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].Description.LastUpdateOn);
			Assert.AreEqual("data", entities[0].Description.Data);

			Assert.AreEqual(initializingDate, entities[0].Things[0].CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].Things[0].LastUpdateOn);
			Assert.AreEqual("data", entities[0].Things[0].Data);
			Assert.AreEqual(admin.Id, entities[0].Things[0].AdminId);
			Assert.AreEqual(entities[0].Id, entities[0].Things[0].ManagerId);

			Assert.AreEqual(initializingDate, entities[0].Things[1].CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].Things[1].LastUpdateOn);
			Assert.AreEqual("data", entities[0].Things[1].Data);
			Assert.AreEqual(admin.Id, entities[0].Things[1].AdminId);
			Assert.AreEqual(entities[0].Id, entities[0].Things[1].ManagerId);

			//Main entity
			Assert.AreEqual("data", entities[0].Data);
			Assert.AreEqual(initializingDate, entities[0].CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].LastUpdateOn);
			Assert.AreEqual(car1.Id, entities[0].CarId);
			Assert.AreEqual(director.Id, entities[0].DirectorId);
			Assert.AreEqual(office1.Id, entities[0].OfficeId);

			//First level used entities
			Assert.AreEqual(office1.Id, entities[0].Office.Id);
			Assert.AreEqual(initializingDate, entities[0].Office.CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].Office.LastUpdateOn);
			Assert.AreEqual("data", entities[0].Office.Data);

			Assert.AreEqual(admin.Id, entities[0].Car.AdminId);
			Assert.AreEqual(car1.Id, entities[0].Car.Id);
			Assert.AreEqual(initializingDate, entities[0].Car.CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].Car.LastUpdateOn);
			Assert.AreEqual("data", entities[0].Car.Data);

			Assert.AreEqual(admin.Id, entities[0].Projects[0].AdminId);
			Assert.AreEqual(project1.Id, entities[0].Projects[0].Id);
			Assert.AreEqual(initializingDate, entities[0].Projects[0].CreatedOn);
			Assert.AreEqual(initializingDate, entities[0].Projects[0].LastUpdateOn);
			Assert.AreEqual("data", entities[0].Projects[0].Data);
			Assert.AreEqual(manager.Id, entities[0].Projects[0].ManagerId);

			//Assert.AreEqual(admin.Id, entities[0].Projects[1].AdminId);
			//Assert.AreEqual(project2.Id, entities[0].Projects[1].Id);
			//Assert.AreEqual(initializingDate, entities[0].Projects[1].CreatedOn);
			//Assert.AreEqual(initializingDate, entities[0].Projects[1].LastUpdateOn);
			//Assert.AreEqual("data", entities[0].Projects[1].Data);
			//Assert.AreEqual(false, entities[0].Projects[1].IsDeleted);
			//Assert.AreEqual(manager.Id, entities[0].Projects[1].ManagerId);

			//Using entities
			Assert.AreEqual(initializingDate, entities[0].ManagerTrackers[0].CreatedOn);
			Assert.AreEqual("data", entities[0].ManagerTrackers[0].Data);
			Assert.AreEqual(initializingDate, entities[0].ManagerTrackers[0].LastUpdateOn);
			Assert.AreEqual(manager.Id, entities[0].ManagerTrackers[0].ManagerId);

			//Get result
		}
	}
}
