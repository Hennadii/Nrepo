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
	public class RelatedEntityItemConfigurationTesting
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WithEntity_RelatedEntityPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers)
				.WithEntity<Director, long?>(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WithEntity_RelatedEntityPropertyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers)
				.WithEntity(e => e.GetType(), e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WithEntity_RelatedEntityPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers)
				.WithEntity(e => e.Director.Data, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WithEntity_RelatedEntityKeyIsNotPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers)
				.WithEntity(e => e.Director, e => e.GetHashCode());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WithEntity_RelatedEntityKeyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers)
				.WithEntity(e => e.Director, e => e.Director.Id);
		}

		[TestMethod]
		public void WithEntity_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId)
				.WithEntities(e => e.Managers).WithEntity(e => e.Director, e => e.DirectorId);
			var result = mock.Object.Entity().GetSubEntityInfo();

			//Assert
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(string.Format("{0}.{1}", ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), ReflectionHelper.GetPropertyName<Manager, Director>(e => e.Director)),
				result[0].RelatedPropertyPath);
			Assert.AreEqual(string.Format("{0}..{1}", ReflectionHelper.GetPropertyName<Manager, long?>(
				e => e.DirectorId), ReflectionHelper.GetPropertyName<Manager, long?>(e => e.DirectorId)),
				result[0].RelatedKeyPath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WithEntities_RelatedEntitiesPropertyIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId)
				.WithEntities(e => e.Managers).WithEntities<Director>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WithEntities_RelatedEntitiesPropertyIsNotOneLevelPropertyExpression_ThrowsArgumentException()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			//Assert
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers)
				.WithEntities(e => e.Director.Managers);
		}

		[TestMethod]
		public void WithEntities_GoodValues_Succeeds()
		{
			//Arrange
			var mock = new Mock<RepositoryTester<Manager>>();
			mock.CallBase = true;

			//Act
			mock.Object.Entity().HasSubEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers)
				.WithEntity(e => e.Director, e => e.DirectorId).WithEntities(e => e.Managers);
			var result = mock.Object.Entity().GetSubEntityInfo();

			//Assert
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(string.Format("{0}.{1}", ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers), ReflectionHelper.GetPropertyName<Manager, IEnumerable<Manager>>(
				e => e.Director.Managers)), result[0].RelatedPropertyPath);
			Assert.AreEqual("DirectorId..DirectorId.", result[0].RelatedKeyPath);
		}
	}
}
