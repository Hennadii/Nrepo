using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nrepo.DataAccess.Properties;
using System.Reflection;

namespace Nrepo.DataAccess.Internal
{
	internal static class DAError
	{
		public static void ArgumentException_IfNotOneLevelPropertyExpression(Expression expression)
		{
			var lambdaExpression = expression as LambdaExpression;

			if (lambdaExpression == null)
			{
				throw new ArgumentException(Resources.NotOneLevelPropertyExpression);
			}

			var memberExpression = lambdaExpression.Body as MemberExpression;

			if (memberExpression == null || memberExpression.ToString().Count(c => c == '.') > 1)
			{
				throw new ArgumentException(Resources.NotOneLevelPropertyExpression);
			}
		}

		public static void ArgumentException_UnknownQueryFilterExpression(QueryFilterExpression expression)
		{
			throw new ArgumentException(string.Format(Resources.UnknownQueryFilterExpression1,
				expression != null ? expression.GetType().FullName : string.Empty));
		}

		public static void EntityNotFoundException_IfEntityNotFoundDuringUpdate<TEntity>(
			TEntity[] entities, object[] primaryKeys)
		{
			if (entities == null || entities.Length == 0)
			{
				throw new EntityNotFoundException(typeof(TEntity), primaryKeys);
			}
		}

		public static void UsingEntityKeyIsNotNullableException_IfUsingEntityKeyIsNotNullable(Type entityType, PropertyInfo keyInfo)
		{
			if ((keyInfo.PropertyType.IsValueType && keyInfo.PropertyType.IsGenericType
				&& keyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) == false
				&& keyInfo.PropertyType.IsValueType)
			{
				throw new UsingEntityKeyIsNotNullableException(entityType, keyInfo.Name);
			}
		}
	}
}
