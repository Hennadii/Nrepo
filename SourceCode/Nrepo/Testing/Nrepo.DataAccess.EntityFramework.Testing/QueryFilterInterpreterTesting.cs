using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nrepo.DataAccess.EntityFramework.Testing.EFRepositoryTesting;

namespace Nrepo.DataAccess.EntityFramework.Testing
{
	[TestClass]
	public class QueryFilterInterpreterTesting
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Interpret_ExpressionIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var interpreter = new QueryFilterInterpreter();

			//Act
			//Assert
			interpreter.Interpret(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Interpret_EntityParameterIsNull_ThrowsArgumentNullException()
		{
			//Arrange
			var interpreter = new QueryFilterInterpreter();

			//Act
			//Assert
			interpreter.Interpret(new NotOperator(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Interpret_ExpressionIsUnknown_ThrowsArgumentException()
		{
			//Arrange
			var interpreter = new QueryFilterInterpreter();

			var expression = new NotOperator()
			{
				Operand = new UnknownCondition()
			};

			//Act
			//Assert
			interpreter.Interpret(expression, Expression.Parameter(typeof(string)));
		}

		[TestMethod]
		public void Interpret_GoodValues_ReturnsExpression()
		{
			//Arrange
			var interpreter = new QueryFilterInterpreter();

			var expression = new NotOperator()
			{
				Operand = new AndOperator()
				{
					LeftOperand = new GreaterThanCondition()
					{
						Property = "Length",
						Value = 0
					},
					RightOperand = new LessThanCondition()
					{
						Property = "Length",
						Value = 6
					}
				}
			};

			var entityParameter = Expression.Parameter(typeof(string));

			//Act
			var result = interpreter.Interpret(expression, entityParameter);

			//Assert
			Assert.AreEqual(ExpressionType.Not, result.NodeType);

			var lambda = Expression.Lambda(Expression.Convert(result, typeof(bool)), entityParameter);

			var value = lambda.Compile().DynamicInvoke("Nice");

			Assert.AreEqual(false, value);
		}
	}
}
