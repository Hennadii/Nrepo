using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nrepo.Internal;

namespace Nrepo
{
	/// <summary>
	/// Provides functionality for working with reflection.
	/// </summary>
	public static class ReflectionHelper
	{
		#region Public Methods

		/// <summary>
		/// Determines whether the base type is inherited.
		/// </summary>
		/// <typeparam name="TBase">The type of the base.</typeparam>
		/// <typeparam name="TDerived">The type of the derived.</typeparam>
		/// <returns><c>true</c> if is inherited, otherwise <c>false</c>.</returns>
		public static bool IsInherited<TBase, TDerived>()
		{
			return typeof(TBase).IsAssignableFrom(typeof(TDerived));
		}

		/// <summary>
		/// Determines whether the specified base type is inherited.
		/// </summary>
		/// <param name="baseType">Type of the base.</param>
		/// <param name="derivedType">Type of the derived.</param>
		/// <returns><c>true</c> if is inherited, otherwise <c>false</c>.</returns>
		public static bool IsInherited(Type baseType, Type derivedType)
		{
			Error.ArgumentNullException_IfNull(baseType, "baseType");
			Error.ArgumentNullException_IfNull(derivedType, "derivedType");

			return baseType.IsAssignableFrom(derivedType);
		}

		/// <summary>
		/// Determines whether the specified base type is inherited from the type of a property.
		/// </summary>
		/// <typeparam name="TBase">The base type.</typeparam>
		/// <param name="derivedType">The derived type.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <returns><c>true</c> if is inherited, otherwise <c>false</c>.</returns>
		public static bool IsInherited<TBase>(Type derivedType, string propertyPath)
		{
			Error.ArgumentNullException_IfNull(derivedType, "derivedType");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(derivedType, propertyPath);

			var pi = GetPropertyInfo(derivedType, propertyPath);

			return typeof(TBase).IsAssignableFrom(pi.PropertyType);
		}

		/// <summary>
		/// Gets the property information.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <returns>The property information.</returns>
		public static PropertyInfo GetPropertyInfo(Type type, string propertyPath)
		{
			Error.ArgumentNullException_IfNull(type, "type");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");

			string[] parts = propertyPath.Split('.');

			return (parts.Length > 1)
				? GetPropertyInfo(type.GetProperty(parts[0]).PropertyType,
					parts.Skip(1).Aggregate((a, i) => a + "." + i))
				: type.GetProperty(propertyPath);
		}

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		/// <typeparam name="T">A type to get the property name of.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="expression">The expression.</param>
		/// <returns>The name of the property.</returns>
		public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
		{
			Error.ArgumentNullException_IfNull(expression, "expression");

			return expression != null
				? string.Join(".", expression.Body.ToString().Split('.').Skip(1))
				.TrimStart('<', '(').TrimEnd('>', ')')
				: null;
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <returns>The property value.</returns>
		public static TProperty GetPropertyValue<TProperty>(object obj, string propertyPath)
		{
			Error.ArgumentNullException_IfNull(obj, "obj");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(obj.GetType(), propertyPath);

			foreach (string propertyName in propertyPath.Split(new char[] { '.' },
				StringSplitOptions.RemoveEmptyEntries))
			{
				if (obj == null)
				{
					return default(TProperty);
				}

				Type currentType = obj.GetType();

				var propertyInfo = currentType.GetProperty(propertyName,
					BindingFlags.Public | BindingFlags.Instance);

				obj = propertyInfo.GetValue(obj, null);
			}

			return obj != null ? (TProperty)obj : default(TProperty);
		}

		/// <summary>
		/// Sets the property value.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="propertyPath">The property path.</param>
		/// <param name="value">The value.</param>
		public static void SetPropertyValue(object obj, string propertyPath, object value)
		{
			Error.ArgumentNullException_IfNull(obj, "obj");
			Error.ArgumentException_IfNullOrEmpty(propertyPath, "propertyPath");
			Error.ArgumentException_IfPropertyNotFound(obj.GetType(), propertyPath);

			PropertyInfo propertyInfo = null;
			object currentObject = null;

			foreach (string propertyName in propertyPath.Split(new char[] { '.' },
				StringSplitOptions.RemoveEmptyEntries))
			{
				if (obj == null)
				{
					return;
				}

				currentObject = obj;

				propertyInfo = obj.GetType().GetProperty(propertyName,
					BindingFlags.Public | BindingFlags.Instance);

				obj = propertyInfo.GetValue(obj, null);
			}

			propertyInfo.SetValue(currentObject, value, null);
		}

		/// <summary>
		/// Removes all references.
		/// </summary>
		/// <param name="obj">The object.</param>
		public static void RemoveAllReferences(object obj)
		{
			Error.ArgumentNullException_IfNull(obj, "obj");

			foreach (var pi in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (!pi.PropertyType.IsValueType && pi.CanWrite && pi.PropertyType != typeof(string))
				{
					pi.SetValue(obj, null, null);
				}
			}
		}

		#endregion
	}
}
