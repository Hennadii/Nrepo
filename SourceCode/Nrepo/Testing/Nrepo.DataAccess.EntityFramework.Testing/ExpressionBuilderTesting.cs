using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nrepo.DataAccess.EntityFramework.Testing
{
	[TestClass]
	public class ExpressionBuilderTesting
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Equals_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Equals(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Equals_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Equals(Expression.Parameter(typeof(string)), string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Equals_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Equals(Expression.Parameter(typeof(string)), "Hello", null);
		}

		[TestMethod]
		public void Equals_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.Equals(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.Equal, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NotEquals_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.NotEquals(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotEquals_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.NotEquals(Expression.Parameter(typeof(string)), string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotEquals_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.NotEquals(Expression.Parameter(typeof(string)), "Hello", null);
		}

		[TestMethod]
		public void NotEquals_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.NotEquals(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.NotEqual, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GreaterThan_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GreaterThan(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GreaterThan_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GreaterThan(Expression.Parameter(typeof(string)), string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GreaterThan_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GreaterThan(Expression.Parameter(typeof(string)), "Hello", null);
		}

		[TestMethod]
		public void GreaterThan_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.GreaterThan(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.GreaterThan, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GreaterThanOrEquals_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GreaterThanOrEquals(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GreaterThanOrEquals_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GreaterThanOrEquals(Expression.Parameter(typeof(string)), string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GreaterThanOrEquals_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GreaterThanOrEquals(Expression.Parameter(typeof(string)), "Hello", null);
		}

		[TestMethod]
		public void GreaterThanOrEquals_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.GreaterThanOrEquals(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.GreaterThanOrEqual, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LessThan_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.LessThan(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void LessThan_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.LessThan(Expression.Parameter(typeof(string)), string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void LessThan_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.LessThan(Expression.Parameter(typeof(string)), "Hello", null);
		}

		[TestMethod]
		public void LessThan_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.LessThan(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.LessThan, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LessThanOrEquals_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.LessThanOrEquals(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void LessThanOrEquals_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.LessThanOrEquals(Expression.Parameter(typeof(string)), string.Empty, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void LessThanOrEquals_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.LessThanOrEquals(Expression.Parameter(typeof(string)), "Hello", null);
		}

		[TestMethod]
		public void LessThanOrEquals_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.LessThanOrEquals(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.LessThanOrEqual, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsNull_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.IsNull(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsNull_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.IsNull(Expression.Parameter(typeof(string)), string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsNull_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.IsNull(Expression.Parameter(typeof(string)), "Hello");
		}

		[TestMethod]
		public void IsNull_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.IsNull(Expression.Parameter(typeof(Type)), "Name");

			//Assert
			Assert.AreEqual(ExpressionType.Equal, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsNotNull_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.IsNotNull(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsNotNull_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.IsNotNull(Expression.Parameter(typeof(string)), string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsNotNull_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.IsNotNull(Expression.Parameter(typeof(string)), "Hello");
		}

		[TestMethod]
		public void IsNotNull_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.IsNotNull(Expression.Parameter(typeof(Type)), "Name");

			//Assert
			Assert.AreEqual(ExpressionType.NotEqual, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Contains_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Contains(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Contains_ValueIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Contains(Expression.Parameter(typeof(string)), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Contains_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Contains(Expression.Parameter(typeof(string)), string.Empty, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Contains_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Contains(Expression.Parameter(typeof(string)), "Hello", 1);
		}

		[TestMethod]
		public void Contains_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.Contains(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.Call, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NotContains_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.NotContains(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NotContains_ValueIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.NotContains(Expression.Parameter(typeof(string)), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotContains_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.NotContains(Expression.Parameter(typeof(string)), string.Empty, 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotContains_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.NotContains(Expression.Parameter(typeof(string)), "Hello", 1);
		}

		[TestMethod]
		public void NotContains_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.NotContains(Expression.Parameter(typeof(string)), "Length", 1);

			//Assert
			Assert.AreEqual(ExpressionType.Not, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void And_LeftIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.And(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void And_RightIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.And(ExpressionBuilder.IsNull(Expression.Parameter(typeof(Type)), "Name"), null);
		}

		[TestMethod]
		public void And_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.And(ExpressionBuilder.IsNull(Expression.Parameter(typeof(Type)), "Name"),
				ExpressionBuilder.IsNotNull(Expression.Parameter(typeof(Type)), "Name"));

			//Assert
			Assert.AreEqual(ExpressionType.And, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Or_LeftIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Or(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Or_RightIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Or(ExpressionBuilder.IsNull(Expression.Parameter(typeof(Type)), "Name"), null);
		}

		[TestMethod]
		public void Or_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.Or(ExpressionBuilder.IsNull(Expression.Parameter(typeof(Type)), "Name"),
				ExpressionBuilder.IsNotNull(Expression.Parameter(typeof(Type)), "Name"));

			//Assert
			Assert.AreEqual(ExpressionType.Or, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Not_ExpressionIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Not(null);
		}

		[TestMethod]
		public void Not_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var expression = ExpressionBuilder.Not(ExpressionBuilder.IsNull(Expression.Parameter(typeof(Type)), "Name"));

			//Assert
			Assert.AreEqual(ExpressionType.Not, expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OrderByAscending_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByAscending(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OrderByAscending_QueryIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByAscending(Expression.Parameter(typeof(string)), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void OrderByAscending_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByAscending(Expression.Parameter(typeof(string)),
				(new string[0]).AsQueryable(), string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void OrderByAscending_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByAscending(Expression.Parameter(typeof(string)),
				(new string[0]).AsQueryable(), "Hello");
		}

		[TestMethod]
		public void OrderByAscending_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var query = ExpressionBuilder.OrderByAscending(Expression.Parameter(typeof(string)),
				(new string[0]).AsQueryable(), "Length");

			//Assert
			Assert.AreEqual(ExpressionType.Call, query.Expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OrderByDescending_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByDescending(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OrderByDescending_QueryIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByDescending(Expression.Parameter(typeof(string)), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void OrderByDescending_PropertyPathIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByDescending(Expression.Parameter(typeof(string)),
				(new string[0]).AsQueryable(), string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void OrderByDescending_PropertyNotExists_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.OrderByDescending(Expression.Parameter(typeof(string)),
				(new string[0]).AsQueryable(), "Hello");
		}

		[TestMethod]
		public void OrderByDescending_GoodValues_ReturnsExpression()
		{
			//Arrange
			//Act
			var query = ExpressionBuilder.OrderByDescending(Expression.Parameter(typeof(string)),
				(new string[0]).AsQueryable(), "Length");

			//Assert
			Assert.AreEqual(ExpressionType.Call, query.Expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Where_QueryIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Where(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Where_ExpressionIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Where((new string[0]).AsQueryable(), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Where_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.Where((new string[0]).AsQueryable(), Expression.Constant(0), null);
		}

		[TestMethod]
		public void Where_GoodValues_ReturnsFilteredQuery()
		{
			//Arrange
			//Act
			var query = ExpressionBuilder.Where((new string[0]).AsQueryable(),
				Expression.Equal(Expression.Constant(0), Expression.Constant(1)),
				Expression.Parameter(typeof(string)));

			//Assert
			Assert.AreEqual(ExpressionType.Call, query.Expression.NodeType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPropertyExpression_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GetPropertyExpression(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetPropertyExpression_PropertyIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GetPropertyExpression(Expression.Parameter(typeof(int)), string.Empty);
		}

		[TestMethod]
		public void GetPropertyExpression_GoodValues_ReturnsPropertyExpression()
		{
			//Arrange
			//Act
			var result = ExpressionBuilder.GetPropertyExpression(Expression.Parameter(typeof(string)), "Length");

			//Assert
			Assert.AreEqual("Length", result.Member.Name);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetPropertyExpression_TEntityTProperty_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GetPropertyExpression<string, int>(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetPropertyExpression_TEntityTProperty_PropertyIsNullOrEmpty_ThrowsArgumentException()
		{
			//Arrange
			//Act
			//Assert
			ExpressionBuilder.GetPropertyExpression<string, int>(Expression.Parameter(typeof(string)), string.Empty);
		}

		[TestMethod]
		public void GetPropertyExpression_TEntityTProperty_GoodValues_ReturnsPropertyExpression()
		{
			//Arrange
			//Act
			var result = ExpressionBuilder.GetPropertyExpression<string, int>(Expression.Parameter(typeof(string)), "Length");

			var value = result.Compile()("asdf");

			//Assert
			Assert.IsTrue(result is LambdaExpression);
			Assert.AreEqual(typeof(string), ((MemberExpression)result.Body).Expression.Type);
			Assert.AreEqual(typeof(int), ((LambdaExpression)result).ReturnType);
			Assert.AreEqual(4, value);
		}
	}
}
