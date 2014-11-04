using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nrepo.DataAccess.Testing.Common;
using Testing.Common;
using Testing.Common.FKSoftDeletableAuditableEntities;

namespace Nrepo.DataAccess.Testing
{
	[TestClass]
	public class RelatedEntityConfigurationTesting
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HasSubEntity_SubEntityPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity<Manager, long?>(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HasSubEntity_SubEntityPropertyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.GetType(), e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HasSubEntity_SubEntityPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Computer.Admin, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HasSubEntity_SubEntityKeyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Computer, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HasSubEntity_SubEntityKeyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Computer, e => e.Computer.Id);
		}

		[TestMethod]
		public void HasSubEntity_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().HasSubEntity(e => e.Computer, e => e.ComputerId);

			var info = mock.Object.Entity().GetSubEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer), info[0].RelatedPropertyPath);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, long?>(e => e.ComputerId), info[0].RelatedKeyPath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HasSubEntities_SubEntitiesPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntities<Manager>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HasSubEntities_SubEntitiesPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntities(e => e.Computer.Admin.Offices);
		}

		[TestMethod]
		public void HasSubEntities_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);

			var info = mock.Object.Entity().GetSubEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), info[0].RelatedPropertyPath);
			Assert.AreEqual("DirectorId.", info[0].RelatedKeyPath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UsesEntity_UsedEntityPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().UsesEntity<Manager, int>(null, (e) => 123);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UsesEntity_UsedEntityPropertyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().UsesEntity(e => e.GetType(), e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UsesEntity_UsedEntityPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().UsesEntity(e => e.Director.Data, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UsesEntity_UsedEntityKeyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().UsesEntity(e => e.Director, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UsesEntity_UsedEntityKeyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().UsesEntity(e => e.Director, e => e.Director.Id);
		}

		[TestMethod]
		public void UsesEntity_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().UsesEntity(e => e.Computer, e => e.ComputerId);

			var info = mock.Object.Entity().GetUsedEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, Computer>(e => e.Computer), info[0].RelatedPropertyPath);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, long?>(e => e.ComputerId), info[0].RelatedKeyPath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UsesEntities_UsedEntitiesPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().UsesEntities<Manager>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UsesEntities_UsedEntitiesPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().UsesEntities(e => e.Director.Managers);
		}

		[TestMethod]
		public void UsesEntities_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().UsesEntity<Director, long>(e => e.Director, null).WithEntities(e => e.Managers);

			var info = mock.Object.Entity().GetUsedEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), info[0].RelatedPropertyPath);
			Assert.AreEqual(".", info[0].RelatedKeyPath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsUsedByEntity_UsingEntityPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().IsUsedByEntity<Manager, int>(null, (e) => 123);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsUsedByEntity_UsingEntityPropertyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().IsUsedByEntity(e => e.GetType(), e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsUsedByEntity_UsingEntityPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().IsUsedByEntity(e => e.Director.Data, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsUsedByEntity_UsingEntityKeyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsUsedByEntity_UsingEntityKeyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.Director.Id);
		}

		[TestMethod]
		public void IsUsedByEntity_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.DirectorId);

			var info = mock.Object.Entity().GetUsingEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director), info[0].RelatedPropertyPath);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, long?>(e => e.DirectorId), info[0].RelatedKeyPath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsUsedByEntities_UsingEntitiesPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().IsUsedByEntities<Manager>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsUsedByEntities_UsingEntitiesPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().IsUsedByEntities(e => e.Director.Managers);
		}

		[TestMethod]
		public void IsUsedByEntities_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);

			var info = mock.Object.Entity().GetUsingEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), info[0].RelatedPropertyPath);
			Assert.AreEqual("DirectorId.", info[0].RelatedKeyPath);
		}

		[TestMethod]
		public void GetSubEntityInfo_Always_ReturnsOptimizedInfo()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId);
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId);

			var info = mock.Object.Entity().GetSubEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), info[0].RelatedPropertyPath);
			Assert.AreEqual("DirectorId.", info[0].RelatedKeyPath);
		}

		[TestMethod]
		public void GetUsedEntityInfo_Always_ReturnsOptimizedInfo()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().UsesEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);
			mock.Object.Entity().UsesEntity(e => e.Director, e => e.DirectorId);
			mock.Object.Entity().UsesEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);
			mock.Object.Entity().UsesEntity(e => e.Director, e => e.DirectorId);

			var info = mock.Object.Entity().GetUsedEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), info[0].RelatedPropertyPath);
			Assert.AreEqual("DirectorId.", info[0].RelatedKeyPath);
		}

		[TestMethod]
		public void GetUsingEntityInfo_Always_ReturnsOptimizedInfo()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();

			mock.CallBase = true;

			//Act
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.DirectorId);
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);
			mock.Object.Entity().IsUsedByEntity(e => e.Director, e => e.DirectorId);

			var info = mock.Object.Entity().GetUsingEntityInfo();

			//Assert
			Assert.AreEqual(1, info.Length);
			Assert.AreEqual(ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), info[0].RelatedPropertyPath);
			Assert.AreEqual("DirectorId.", info[0].RelatedKeyPath);
		}
	}
}
