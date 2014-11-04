using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nrepo.DataAccess.Testing
{
	[TestClass]
	public class RelatedEntityInfoTesting
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RelatedEntityInfo_RelatedPropertyPathIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new RelatedEntityInfo(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RelatedEntityInfo_RelatedKeyPathIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			new RelatedEntityInfo(string.Empty, null);
		}

		[TestMethod]
		public void RelatedEntityInfo_InitializesEmptyInstance()
		{
			//Arrange
			//Act
			var info = new RelatedEntityInfo();

			//Assert
			Assert.IsTrue(info.IsEmpty());
		}

		[TestMethod]
		public void IsEmpty_IsEmpty_ReturnsTrue()
		{
			//Arrange
			//Act
			var info = new RelatedEntityInfo(string.Empty, string.Empty);

			//Assert
			Assert.IsTrue(info.IsEmpty());
		}

		[TestMethod]
		public void Equals_Equals_ReturnsTrue()
		{
			//Arrange
			var info1 = new RelatedEntityInfo("a.x.b", "aId..bId");
			var info2 = new RelatedEntityInfo("a.x.b", "aId..bId");

			//Act
			var equal = info1.Equals(info2);

			//Assert
			Assert.IsTrue(equal);
		}

		[TestMethod]
		public void Equals_NotEquals_ReturnsFalse()
		{
			//Arrange
			var info1 = new RelatedEntityInfo("a.x.b", "aId..bId");
			var info2 = new RelatedEntityInfo("a.c.b", "aId..bId");

			//Act
			var equal = info1.Equals(info2);

			//Assert
			Assert.IsFalse(equal);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Combine_FirstInfoIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			RelatedEntityInfo.Combine(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Combine_SecondInfoIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			RelatedEntityInfo.Combine(new RelatedEntityInfo(), null);
		}

		[TestMethod]
		public void Combine_FirstInfoIsEmpty_ReturnsSecondInfo()
		{
			//Arrange
			var firstInfo = new RelatedEntityInfo();
			var secondInfo = new RelatedEntityInfo("a.c.b", "aId..bId");

			//Act
			var result = RelatedEntityInfo.Combine(firstInfo, secondInfo);

			//Assert
			Assert.AreEqual(secondInfo, result);
		}

		[TestMethod]
		public void Combine_SecondInfoIsEmpty_ReturnsFirstInfo()
		{
			//Arrange
			var firstInfo = new RelatedEntityInfo("a.c.b", "aId..bId");
			var secondInfo = new RelatedEntityInfo();

			//Act
			var result = RelatedEntityInfo.Combine(firstInfo, secondInfo);

			//Assert
			Assert.AreEqual(firstInfo, result);
		}

		[TestMethod]
		public void Combine_GoodValues_ReturnsCombinedInfo()
		{
			//Arrange
			var firstInfo = new RelatedEntityInfo("a.c.b", "aId..bId");
			var secondInfo = new RelatedEntityInfo("b.c", "bId.");

			//Act
			var result = RelatedEntityInfo.Combine(firstInfo, secondInfo);

			//Assert
			Assert.AreEqual(new RelatedEntityInfo("a.c.b.b.c", "aId..bId.bId."), result);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Includes_OtherIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var info = new RelatedEntityInfo("a.c.b", "aId..bId");

			//Act
			//Assert
			var result = info.Includes(null);
		}

		[TestMethod]
		public void Includes_WhenNotIncludes_ReturnsFalse()
		{
			//Arrange
			var info = new RelatedEntityInfo("a.c.b", "aId..bId");
			var other = new RelatedEntityInfo("a.x", ".xId");

			//Act
			var result = info.Includes(other);

			//Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Includes_WhenIncludes_ReturnsTrue()
		{
			//Arrange
			var info = new RelatedEntityInfo("a.c.b", "aId..bId");
			var other = new RelatedEntityInfo("a.c", "aId.");

			//Act
			var result = info.Includes(other);

			//Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void First_WhenEmpty_ReturnsEmpty()
		{
			//Arrange
			var info = new RelatedEntityInfo();

			//Act
			var result = info.First();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo(), result);
		}

		[TestMethod]
		public void First_WhenNotEmpty_ReturnsFirst()
		{
			//Arrange
			var info = new RelatedEntityInfo("a.x", ".xId");

			//Act
			var result = info.First();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo("a", string.Empty), result);
		}

		[TestMethod]
		public void Last_WhenEmpty_ReturnsEmpty()
		{
			//Arrange
			var info = new RelatedEntityInfo();

			//Act
			var result = info.Last();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo(), result);
		}

		[TestMethod]
		public void Last_WhenNotEmpty_ReturnsLast()
		{
			//Arrange
			var info1 = new RelatedEntityInfo("a.bbb.cc", "aId..ccId");
			var info2 = new RelatedEntityInfo("a.bbb.cc", "aId..");

			//Act
			var result1 = info1.Last();
			var result2 = info2.Last();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo("cc", "ccId"), result1);
			Assert.AreEqual(new RelatedEntityInfo("cc", string.Empty), result2);
		}

		[TestMethod]
		public void WithoutFirst_WhenEmpty_ReturnsEmpty()
		{
			//Arrange
			var info = new RelatedEntityInfo();

			//Act
			var result = info.WithoutFirst();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo(), result);
		}

		[TestMethod]
		public void WithoutFirst_WhenNotEmpty_ReturnsWithoutFirst()
		{
			//Arrange
			var info = new RelatedEntityInfo("a.bbb.cc", "aId..ccId");

			//Act
			var result = info.WithoutFirst();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo("bbb.cc", ".ccId"), result);
		}

		[TestMethod]
		public void WithoutLast_WhenEmpty_ReturnsEmpty()
		{
			//Arrange
			var info = new RelatedEntityInfo();

			//Act
			var result = info.WithoutFirst();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo(), result);
		}

		[TestMethod]
		public void WithoutLast_WhenNotEmpty_ReturnsWithoutLast()
		{
			//Arrange
			var info = new RelatedEntityInfo("a.bbb.cc", "aId..ccId");

			//Act
			var result = info.WithoutLast();

			//Assert
			Assert.AreEqual(new RelatedEntityInfo("a.bbb", "aId."), result);
		}
	}
}
