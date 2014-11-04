using System;
using System.Linq.Expressions;
using Nrepo.DataAccess.Internal;
using Nrepo.Internal;

namespace Nrepo.DataAccess.EntityFramework
{
	/// <summary>
	/// Represents the default query filter interpreter, which builds a Linq To SQL expression
	/// from the given query filtering expression.
	/// </summary>
	public class QueryFilterInterpreter
	{
		#region Public Methods

		/// <summary>
		/// Builds a Linq To SQL expression from the given query filtering expression.
		/// </summary>
		/// <param name="expression">An expression to interpret.</param>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <returns>A Linq To SQL expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="expression"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="expression"/> contains an unknown filtering expression.</exception>
		public virtual Expression Interpret(QueryFilterExpression expression, ParameterExpression entityParameter)
		{
			Error.ArgumentNullException_IfNull(expression, "expression");
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");

			var result = InterpretCondition(ExpressionBuilder.Contains, expression as ContainsCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.NotContains, expression as NotContainsCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.Equals, expression as EqualsCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.NotEquals, expression as NotEqualsCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.GreaterThan, expression as GreaterThanCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.GreaterThanOrEquals, expression as GreaterThanOrEqualsCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.LessThan, expression as LessThanCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.LessThanOrEquals, expression as LessThanOrEqualsCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.IsNull, expression as IsNullCondition, entityParameter)
				?? InterpretCondition(ExpressionBuilder.IsNotNull, expression as IsNotNullCondition, entityParameter)
				?? InterpretOperator(ExpressionBuilder.And, expression as AndOperator, entityParameter)
				?? InterpretOperator(ExpressionBuilder.Or, expression as OrOperator, entityParameter)
				?? InterpretOperator(ExpressionBuilder.Not, expression as NotOperator, entityParameter);

			if (result != null)
			{
				return result;
			}

			DAError.ArgumentException_UnknownQueryFilterExpression(expression);

			return null;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Interprets the condition.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <returns>The interpreted condition.</returns>
		private Expression InterpretCondition(Func<ParameterExpression, string, object, Expression> action,
			QueryFilterComparingCondition condition, ParameterExpression entityParameter)
		{
			return condition != null
				? action(entityParameter, condition.Property, condition.Value)
				: null;
		}

		/// <summary>
		/// Interprets the condition.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <returns>The interpreted condition.</returns>
		private Expression InterpretCondition(Func<ParameterExpression, string, Expression> action,
			QueryFilterCondition condition, ParameterExpression entityParameter)
		{
			return condition != null
				? action(entityParameter, condition.Property)
				: null;
		}

		/// <summary>
		/// Interprets the binary operator.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="binaryOperator">The binary operator.</param>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <returns>The interpreted condition.</returns>
		private Expression InterpretOperator(Func<Expression, Expression, Expression> action,
			QueryFilterBinaryOperator binaryOperator, ParameterExpression entityParameter)
		{
			return binaryOperator != null
				? action(Interpret(binaryOperator.LeftOperand, entityParameter),
				Interpret(binaryOperator.RightOperand, entityParameter))
				: null;
		}

		/// <summary>
		/// Interprets the binary operator.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="unaryOperator">The binary operator.</param>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <returns>The interpreted condition.</returns>
		private Expression InterpretOperator(Func<Expression, Expression> action,
			QueryFilterUnaryOperator unaryOperator, ParameterExpression entityParameter)
		{
			return unaryOperator != null
				? action(Interpret(unaryOperator.Operand, entityParameter))
				: null;
		}

		#endregion
	}
}
