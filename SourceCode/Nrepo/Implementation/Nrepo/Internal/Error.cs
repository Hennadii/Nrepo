using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nrepo.Properties;

namespace Nrepo.Internal
{
	internal static class Error
	{
		public static void ArgumentNullException_IfNull(object argument, string argumentName)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(string.Format(
					Resources.ArgumentIsNull1, argumentName ?? string.Empty));
			}
		}

		public static void ArgumentException_IfNullOrEmpty(string argument, string argumentName)
		{
			if (string.IsNullOrEmpty(argument))
			{
				throw new ArgumentException(string.Format(
					Resources.ArgumentIsNullOrEmpty1, argumentName ?? string.Empty));
			}
		}

		public static void ArgumentException_IfPropertyIsNull<T>(
			T argument, Expression<Func<T, object>> propertyExpression)
		{
			object property = null;

			if (propertyExpression == null)
			{
				throw new ArgumentException(string.Format(Resources.PropertyIsNull1, string.Empty));
			}

			try
			{
				property = propertyExpression.Compile()(argument);
			}
			catch (NullReferenceException)
			{
				throw new ArgumentException(string.Format(
					Resources.PropertyIsNull1, string.Join(".",
					propertyExpression.ToString().Split('.').Skip(1))));
			}

			if (property == null)
			{
				throw new ArgumentException(string.Format(Resources.PropertyIsNull1, string.Empty));
			}
		}

		public static void ArgumentException_IfPropertyNotFound(Type type, string propertyPath)
		{
			if (string.IsNullOrEmpty(propertyPath))
			{
				throw new ArgumentException(string.Format(
					Resources.PropertyNotFound1, string.Empty));
			}

			PropertyInfo pi = null;

			try
			{
				pi = ReflectionHelper.GetPropertyInfo(type, propertyPath);
			}
			catch (Exception)
			{
				throw new ArgumentException(string.Format(
					Resources.PropertyNotFound1, propertyPath));
			}

			if (pi == null)
			{
				throw new ArgumentException(string.Format(
					Resources.PropertyNotFound1, propertyPath));
			}
		}
	}
}
