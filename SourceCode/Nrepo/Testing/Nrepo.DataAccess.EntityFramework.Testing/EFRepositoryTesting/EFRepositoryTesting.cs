using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nrepo.DataAccess.EntityFramework.Testing.EFRepositoryTesting;

namespace Nrepo.DataAccess.EntityFramework.EFRepositoryTesting
{
	[TestClass]
	public class EFRepositoryTesting
	{
		[ClassCleanup]
		public static void ClassCleanup()
		{
			EFContext.DeleteDatabase();
		}

		[TestMethod]
		public void CreateQueryFilterInterpreter_Always_RetursQueryFilterInterpreter()
		{
			//Arrange
			//Act
			var interpreter = new EFRepositoryTester().CreateQueryFilterInterpreter();

			//Assert
			Assert.AreEqual(typeof(QueryFilterInterpreter), interpreter.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Add_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().Add(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Update_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().Update(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Delete_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().Delete(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Get_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().Get(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadRelatedEntities_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().LoadRelatedEntities(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupAdd_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupAdd(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupAdd_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupAdd(new EFContext(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupUpdate_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupUpdate(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupUpdate_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupUpdate(new EFContext(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupDelete_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupDelete(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupDelete_ParametersIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupDelete(new EFContext(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetQueryFiltering_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetQueryFiltering(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetQueryFiltering_QueryIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetQueryFiltering(new EFContext(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetQueryPaging_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetQueryPaging(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetQueryPaging_QueryIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetQueryPaging(new EFContext(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetTotalCount_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetTotalCount(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetTotalCount_QueryIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetTotalCount(new EFContext(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetQuerySorting_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetQuerySorting(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetupGetQuerySorting_QueryIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SetupGetQuerySorting(new EFContext(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPrimaryKeyNames_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().GetPrimaryKeyNames(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPrimaryKeyNames_EntityTypeIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().GetPrimaryKeyNames(new EFContext(), null);
		}

		[TestMethod]
		public void GetPrimaryKeyNames_GoodValues_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();

			//Act
			var result = repository.GetPrimaryKeyNames(new EFContext(), typeof(EFEntity));

			//Assert
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<EFEntity, long>(e => e.Id), result[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetFilterByPrimaryKeys_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().GetFilterByPrimaryKeys(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetFilterByPrimaryKeys_EntityTypeIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().GetFilterByPrimaryKeys(new EFContext(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetFilterByPrimaryKeys_PrimaryKeysIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().GetFilterByPrimaryKeys(new EFContext(), typeof(EFEntity), null);
		}

		[TestMethod]
		public void GetFilterByPrimaryKeys_GoodValues_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();

			//Act
			var result = repository.GetFilterByPrimaryKeys(new EFContext(), typeof(EFEntity), new object[] { 123L });

			//Assert
			Assert.AreEqual(typeof(EqualsCondition), result.GetType());
			Assert.AreEqual(ReflectionHelper.GetPropertyName<EFEntity, long>(e => e.Id), ((EqualsCondition)result).Property);
			Assert.AreEqual(123L, ((EqualsCondition)result).Value);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AreEntitiesEqual_ContextIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().AreEntitiesEqual(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AreEntitiesEqual_Entity1IsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().AreEntitiesEqual(new EFContext(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AreEntitiesEqual_Entity2IsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().AreEntitiesEqual(new EFContext(), new EFEntity(), null);
		}

		[TestMethod]
		public void AreEntitiesEqual_GoodValues_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();

			//Act
			var result1 = repository.AreEntitiesEqual(new EFContext(), new EFEntity() { Id = 1 }, new EFEntity() { Id = 1 });
			var result2 = repository.AreEntitiesEqual(new EFContext(), new EFEntity() { Id = 2 }, new EFEntity() { Id = 1 });

			//Assert
			Assert.IsTrue(result1);
			Assert.IsFalse(result2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SynchronizeForeignKeysWithEntities_RootEntityIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new EFRepositoryTester().SynchronizeForeignKeysWithEntities(null);
		}

		[TestMethod]
		public void SynchronizeForeignKeysWithEntities_GoodValues_Succeeds()
		{
			//Arrange
			var repository = new EFRepositoryTester();

			var entity = new EFEntity()
			{
				Id = 1,
				EFEntityChildren = new List<EFEntityChild>()
				{
					new EFEntityChild()
					{
						Id = 1,
					},
					new EFEntityChild()
					{
						Id = 2,
					}
				}
			};

			//Act
			repository.SynchronizeForeignKeysWithEntities(entity);

			//Assert
			Assert.AreEqual(1, entity.Id);
			Assert.AreEqual(2, entity.EFEntityChildren.Count);

			Assert.AreEqual(1, entity.EFEntityChildren[0].Id);
			Assert.AreEqual(1, entity.EFEntityChildren[0].EFEntityId);
			Assert.AreEqual(entity, entity.EFEntityChildren[0].EFEntity);

			Assert.AreEqual(2, entity.EFEntityChildren[1].Id);
			Assert.AreEqual(1, entity.EFEntityChildren[1].EFEntityId);
			Assert.AreEqual(entity, entity.EFEntityChildren[1].EFEntity);
		}
	}
}
