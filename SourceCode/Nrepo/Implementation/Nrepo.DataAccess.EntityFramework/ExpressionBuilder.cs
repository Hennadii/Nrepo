using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using Nrepo.Internal;

namespace Nrepo.DataAccess.EntityFramework
{
	/// <summary>
	/// Provides methods for building expressions abainst database.
	/// </summary>
	public static class ExpressionBuilder
	{
		#region Conditions

		/// <summary>
		/// Builds the "equals" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "equals" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression Equals(ParameterExpression entityParameter, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "property");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.Equal(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(value));
		}

		/// <summary>
		/// Builds the "not equals" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "not equals" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression NotEquals(ParameterExpression entityParameter, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.NotEqual(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(value));
		}

		/// <summary>
		/// Builds the "contains" expression.
		/// Uses the <see cref="System.Data.Entity.SqlServer.SqlFunctions.StringConvert(double?)"/>.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "contains" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/>
		/// or <paramref name="value"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression Contains(ParameterExpression entityParameter, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentNullException_IfNull(value, "value");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			if (value.GetType() == typeof(string))
			{
				return Expression.Call(GetPropertyExpression(entityParameter, propertyPath),
					typeof(string).GetMethod("Contains"), Expression.Constant(value));
			}
			else
			{
				string stringValue = value.ToString();

				var stringConvert = typeof(SqlFunctions).GetMethod(
					"StringConvert", new Type[] { typeof(double?) });

				var stringContains = typeof(string).GetMethod("Contains");

				return Expression.Call(Expression.Call(stringConvert, Expression.Convert(
					GetPropertyExpression(entityParameter, propertyPath), typeof(double?))),
					stringContains, Expression.Constant(stringValue));
			}
		}

		/// <summary>
		/// Builds the " not contains" expression.
		/// Uses the <see cref="System.Data.Entity.SqlServer.SqlFunctions.StringConvert(double?)"/>.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "not contains" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/>
		/// or <paramref name="value"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression NotContains(ParameterExpression entityParameter, string propertyPath, object value)
		{
			return ExpressionBuilder.Not(ExpressionBuilder.Contains(entityParameter, propertyPath, value));
		}

		/// <summary>
		/// Builds the "greater than" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "greater than" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression GreaterThan(ParameterExpression entityParameter, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.GreaterThan(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(value));
		}

		/// <summary>
		/// Builds the "greater than or equals" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "greater than or equals" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression GreaterThanOrEquals(ParameterExpression entityParameter, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.GreaterThanOrEqual(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(value));
		}

		/// <summary>
		/// Builds the "less than" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "less than" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression LessThan(ParameterExpression entityParameter, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.LessThan(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(value));
		}

		/// <summary>
		/// Builds the "less than or equals" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		/// <returns>The "less than or equals" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression LessThanOrEquals(ParameterExpression entityParameter, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.LessThanOrEqual(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(value));
		}

		/// <summary>
		/// Builds the "is null" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <returns>The "is null" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression IsNull(ParameterExpression entityParameter, string propertyPath)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.Equal(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(null));
		}

		/// <summary>
		/// Builds the "is not null" expression.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <returns>The "is not null" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static Expression IsNotNull(ParameterExpression entityParameter, string propertyPath)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			return Expression.NotEqual(GetPropertyExpression(entityParameter, propertyPath), Expression.Constant(null));
		}

		#endregion

		#region Operators

		/// <summary>
		/// Builds the "and" expression.
		/// </summary>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The "and" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="left"/>
		/// or <paramref name="right"/> is null.</exception>
		public static Expression And(Expression left, Expression right)
		{
			Error.ArgumentNullException_IfNull(left, "left");
			Error.ArgumentNullException_IfNull(right, "right");

			return Expression.And(left, right);
		}

		/// <summary>
		/// Builds the "or" expression.
		/// </summary>
		/// <param name="left">The left expression.</param>
		/// <param name="right">The right expression.</param>
		/// <returns>The "or" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="left"/>
		/// or <paramref name="right"/> is null.</exception>
		public static Expression Or(Expression left, Expression right)
		{
			Error.ArgumentNullException_IfNull(left, "left");
			Error.ArgumentNullException_IfNull(right, "right");

			return Expression.Or(left, right);
		}

		/// <summary>
		/// Builds the "not" expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns>The "not" expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="expression"/> is null.</exception>
		public static Expression Not(Expression expression)
		{
			Error.ArgumentNullException_IfNull(expression, "expression");

			return Expression.Not(expression);
		}

		#endregion

		#region Sorting

		/// <summary>
		/// Extends the specified query with ordering by ascending.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="query">The query.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <returns>The ordered query.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/>
		/// or <paramref name="query"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static IQueryable OrderByAscending(ParameterExpression entityParameter, IQueryable query, string propertyPath)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentNullException_IfNull(query, "query");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			var pi = ReflectionHelper.GetPropertyInfo(entityParameter.Type, propertyPath);

			return query.Provider.CreateQuery(Expression.Call(typeof(Queryable), "OrderBy",
				new Type[] { entityParameter.Type, pi.PropertyType }, query.Expression,
				Expression.Lambda(GetPropertyExpression(entityParameter, propertyPath), entityParameter)));
		}

		/// <summary>
		/// Extends the specified query with ordering by descending.
		/// </summary>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <param name="query">The query.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <returns>The ordered query.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/>
		/// or <paramref name="query"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="propertyPath"/> is null or empty
		/// or the property was not found.</exception>
		public static IQueryable OrderByDescending(ParameterExpression entityParameter, IQueryable query, string propertyPath)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentNullException_IfNull(query, "query");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(entityParameter.Type, propertyPath);

			var pi = ReflectionHelper.GetPropertyInfo(entityParameter.Type, propertyPath);

			return query.Provider.CreateQuery(Expression.Call(typeof(Queryable), "OrderByDescending",
				new Type[] { entityParameter.Type, pi.PropertyType }, query.Expression,
				Expression.Lambda(GetPropertyExpression(entityParameter, propertyPath), entityParameter)));
		}

		#endregion

		#region Filtering

		/// <summary>
		/// Extends the specified query with the filtering expression.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="expression">The expression.</param>
		/// <param name="entityParameter">The expression entity parameter.</param>
		/// <returns>The filtered query.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="query"/>
		/// or <paramref name="expression"/> is null.</exception>
		public static IQueryable Where(IQueryable query, Expression expression, ParameterExpression entityParameter)
		{
			Error.ArgumentNullException_IfNull(query, "query");
			Error.ArgumentNullException_IfNull(expression, "expression");
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");

			var lambda = Expression.Lambda(Expression.Convert(expression, typeof(bool)), entityParameter);

			return query.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Where",
				new Type[] { query.ElementType }, query.Expression, lambda));
		}

		#endregion

		#region Common Methods

		/// <summary>
		/// Gets the property expression.
		/// </summary>
		/// <param name="entityParameter">The entity parameter.</param>
		/// <param name="property">The property.</param>
		/// <returns>The property expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="property"/> is null or empty.</exception>
		public static MemberExpression GetPropertyExpression(ParameterExpression entityParameter, string property)
		{
			Error.ArgumentNullException_IfNull(entityParameter, "entityParameter");
			Error.ArgumentException_IfNullOrEmpty(property, "property");

			MemberExpression propertyExpression = null;

			string[] propertyNames = property.Split('.');

			foreach (string memberName in propertyNames)
			{
				if (propertyExpression == null)
				{
					propertyExpression = Expression.Property(entityParameter, memberName);
				}
				else
				{
					propertyExpression = Expression.Property(propertyExpression, memberName);
				}
			}

			return propertyExpression;
		}

		/// <summary>
		/// Gets the property expression.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="entityParameter">The entity parameter.</param>
		/// <param name="property">The property.</param>
		/// <returns>The property expression.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="entityParameter"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="property"/> is null or empty.</exception>
		public static Expression<Func<TEntity, TProperty>> GetPropertyExpression<TEntity, TProperty>(
			ParameterExpression entityParameter, string property)
		{
			return Expression.Lambda<Func<TEntity, TProperty>>(
				GetPropertyExpression(entityParameter, property), entityParameter);
		}

		#endregion
	}
}
