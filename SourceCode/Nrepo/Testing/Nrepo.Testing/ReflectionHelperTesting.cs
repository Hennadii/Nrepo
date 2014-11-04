using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Common.FKSoftDeletableAuditableEntities;

namespace Nrepo.Testing
{
	[TestClass]
	public class ReflectionHelperTesting
	{
		[TestMethod]
		public void IsInherited_TBase_TDerived_BadValues_ReturnsFalse()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.IsInherited<string, object>();

			//Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void IsInherited_TBase_TDerived_GoodValues_ReturnsTrue()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.IsInherited<object, string>();

			//Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsInherited_BaseTypeIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.IsInherited(null, typeof(object));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsInherited_DerivedTypeIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.IsInherited(typeof(string), null);
		}

		[TestMethod]
		public void IsInherited_FalseValues_ReturnsFalse()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.IsInherited(typeof(string), typeof(object));

			//Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void IsInherited_TrueValues_ReturnsTrue()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.IsInherited(typeof(object), typeof(string));

			//Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsInherited_TBase_DerivedTypeIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.IsInherited<long>(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsInherited_TBase_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.IsInherited<long>(typeof(string), string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsInherited_TBase_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.IsInherited<long>(typeof(string), "Length.Hello");
		}

		[TestMethod]
		public void IsInherited_TBase_FalseValues_ReturnsFalse()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.IsInherited<long>(typeof(string),
				ReflectionHelper.GetPropertyName<string, int>(s => s.Length));

			//Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void IsInherited_TBase_TrueValues_ReturnsTrue()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.IsInherited<object>(typeof(string),
				ReflectionHelper.GetPropertyName<string, int>(s => s.Length));

			//Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPropertyInfo_TypeIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.GetPropertyInfo(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetPropertyInfo_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.GetPropertyInfo(typeof(string), string.Empty);
		}

		[TestMethod]
		public void GetPropertyInfo_PropertyPathIsInvalid_ReturnsNull()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.GetPropertyInfo(typeof(string), "Hello");

			//Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetPropertyInfo_GoodValues_ReturnsPropertyInfo()
		{
			//Arrange
			//Act
			var result = ReflectionHelper.GetPropertyInfo(typeof(string), "Length");

			//Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPropertyName_ExpressionIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.GetPropertyName<string, int>(null);
		}

		[TestMethod]
		public void GetPropertyName_GoodValues_ReturnsPropertyName()
		{
			//Arrange
			//Act
			var name = ReflectionHelper.GetPropertyName<string, int>(s => s.Length);

			//Assert
			Assert.AreEqual("Length", name);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPropertyValue_ObjIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.GetPropertyValue<object>(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetPropertyValue_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.GetPropertyValue<object>(string.Empty, string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetPropertyValue_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.GetPropertyValue<object>(string.Empty, "Hello");
		}

		[TestMethod]
		public void GetPropertyValue_PropertyEqualsNull_ReturnsNull()
		{
			//Arrange
			var manager = new Manager();

			//Act
			var value = ReflectionHelper.GetPropertyValue<object>(manager,
				ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description));

			//Assert
			Assert.IsNull(value);
		}

		[TestMethod]
		public void GetPropertyValue_PropertyIsNotNull_ReturnsValue()
		{
			//Arrange
			var manager = new Manager();
			var description = new Description();
			manager.Description = description;

			//Act
			var value = ReflectionHelper.GetPropertyValue<object>(manager,
				ReflectionHelper.GetPropertyName<Manager, Description>(e => e.Description));

			//Assert
			Assert.AreEqual(description, value);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetPropertyValue_ObjIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.SetPropertyValue(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SetPropertyValue_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.SetPropertyValue(string.Empty, string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SetPropertyValue_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.SetPropertyValue(string.Empty, "Hello", null);
		}

		[TestMethod]
		public void SetPropertyValue_PropertyExists_Succeeds()
		{
			//Arrange
			var manager = new Manager()
			{
				Director = new Director()
				{
					Managers = null
				}
			};

			//Act
			ReflectionHelper.SetPropertyValue(manager, ReflectionHelper
				.GetPropertyName<Manager, IList<Manager>>(e => e.Director.Managers),
				new List<Manager>() { new Manager() { Id = 123 } });

			//Assert
			Assert.AreEqual(123L, manager.Director.Managers[0].Id);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveAllReferences_ObjIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ReflectionHelper.RemoveAllReferences(null);
		}

		[TestMethod]
		public void RemoveAllReferences_GoodValues_Succeeds()
		{
			//Arrange
			var date = DateTime.UtcNow;

			var obj = new Manager()
			{
				Car = new Car(),
				CarId = 1,
				Computer = new Computer(),
				ComputerId = 2,
				CreatedById = 3,
				CreatedOn = date,
				Data = "data",
				Description = new Description(),
				DescriptionId = 4,
				Director = new Director(),
				DirectorId = 5,
				Id = 6,
				IsDeleted = false,
				LastUpdatedById = 7,
				LastUpdateOn = date,
				ManagerTrackers = new List<ManagerTracker>()
				{
					new ManagerTracker()
				},
				Office = new Office(),
				OfficeId = 8,
				Projects = new List<Project>()
				{
					new Project()
				},
				Things = new List<Thing>()
				{
					new Thing()
				}
			};

			//Act
			ReflectionHelper.RemoveAllReferences(obj);

			//Assert
			Assert.AreEqual(null, obj.Car);
			Assert.AreEqual(1, obj.CarId);
			Assert.AreEqual(null, obj.Computer);
			Assert.AreEqual(2, obj.ComputerId);
			Assert.AreEqual(3, obj.CreatedById);
			Assert.AreEqual(date, obj.CreatedOn);
			Assert.AreEqual("data", obj.Data);
			Assert.AreEqual(null, obj.Description);
			Assert.AreEqual(4, obj.DescriptionId);
			Assert.AreEqual(null, obj.Director);
			Assert.AreEqual(5, obj.DirectorId);
			Assert.AreEqual(6, obj.Id);
			Assert.AreEqual(false, obj.IsDeleted);
			Assert.AreEqual(7, obj.LastUpdatedById);
			Assert.AreEqual(date, obj.LastUpdateOn);
			Assert.AreEqual(null, obj.ManagerTrackers);
			Assert.AreEqual(null, obj.Office);
			Assert.AreEqual(8, obj.OfficeId);
			Assert.AreEqual(null, obj.Projects);
			Assert.AreEqual(null, obj.Things);
		}
	}
}
