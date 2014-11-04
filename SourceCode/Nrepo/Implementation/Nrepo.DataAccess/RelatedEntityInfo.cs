using System;
using System.Linq;
using Nrepo.Internal;

namespace Nrepo.DataAccess
{
	/// <summary>
	/// Provides information about a related entity.
	/// </summary>
	public sealed class RelatedEntityInfo
	{
		#region Fields

		private string[] relatedPropertyPathItems;

		private string[] relatedKeyPathItems;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RelatedEntityInfo"/> class.
		/// </summary>
		/// <param name="relatedPropertyPath">The related property path.</param>
		/// <param name="relatedKeyPath">The related key path.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="relatedPropertyPath"/> or
		/// <paramref name="relatedKeyPath"/> is null.</exception>
		public RelatedEntityInfo(string relatedPropertyPath, string relatedKeyPath)
		{
			Error.ArgumentNullException_IfNull(relatedPropertyPath, "relatedPropertyPath");
			Error.ArgumentNullException_IfNull(relatedKeyPath, "relatedKeyPath");

			RelatedPropertyPath = relatedPropertyPath;
			RelatedKeyPath = relatedKeyPath;

			relatedPropertyPathItems = RelatedPropertyPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			relatedKeyPathItems = RelatedKeyPath.Split(new char[] { '.' }, StringSplitOptions.None);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelatedEntityInfo"/> class.
		/// </summary>
		public RelatedEntityInfo()
			: this(string.Empty, string.Empty)
		{
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Determines whether this instance is empty.
		/// </summary>
		/// <returns><c>true</c> if this instance is empty, otherwise <c>false</c>.</returns>
		public bool IsEmpty()
		{
			return RelatedPropertyPath == string.Empty;
		}

		/// <summary>
		/// Gets the first item in this information.
		/// </summary>
		/// <returns>The first item in this information.</returns>
		public RelatedEntityInfo First()
		{
			if (relatedPropertyPathItems.Length == 0)
			{
				return new RelatedEntityInfo();
			}

			return new RelatedEntityInfo(relatedPropertyPathItems[0], relatedKeyPathItems[0]);
		}

		/// <summary>
		/// Gets the last item in this information.
		/// </summary>
		/// <returns>The last item in this information.</returns>
		public RelatedEntityInfo Last()
		{
			if (relatedPropertyPathItems.Length == 0)
			{
				return new RelatedEntityInfo();
			}

			return new RelatedEntityInfo(relatedPropertyPathItems[relatedPropertyPathItems.Length - 1],
				relatedKeyPathItems[relatedKeyPathItems.Length - 1]);
		}

		/// <summary>
		/// Gets the relatedPropertyPathItems without first item in this information.
		/// </summary>
		/// <returns>The relatedPropertyPathItems without first item in this information.</returns>
		public RelatedEntityInfo WithoutFirst()
		{
			if (relatedPropertyPathItems.Length == 0)
			{
				return new RelatedEntityInfo();
			}

			var keyPath = RelatedKeyPath.Substring(relatedKeyPathItems[0].Length);
			if (keyPath.StartsWith("."))
			{
				keyPath = keyPath.Remove(0, 1);
			}

			return new RelatedEntityInfo(RelatedPropertyPath.Substring(
				relatedPropertyPathItems[0].Length).TrimStart('.'), keyPath);
		}

		/// <summary>
		/// Gets the relatedPropertyPathItems without last item in this information.
		/// </summary>
		/// <returns>The relatedPropertyPathItems without last item in this information.</returns>
		public RelatedEntityInfo WithoutLast()
		{
			if (relatedPropertyPathItems.Length == 0)
			{
				return new RelatedEntityInfo();
			}

			var propertyPath = RelatedPropertyPath.Substring(0, RelatedPropertyPath.Length
				- relatedPropertyPathItems[relatedPropertyPathItems.Length - 1].Length).TrimEnd('.');

			var keyPath = RelatedKeyPath.Substring(0, RelatedKeyPath.Length
				- relatedKeyPathItems[relatedKeyPathItems.Length - 1].Length);

			if (keyPath.EndsWith("."))
			{
				keyPath = keyPath.Remove(keyPath.Length - 1, 1);
			}

			return new RelatedEntityInfo(propertyPath, keyPath);
		}

		/// <summary>
		/// Checks if the current information includes the specified information.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if the current information includes the specified information, otherwise <c>false</c>.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="other"/> is null.</exception>
		public bool Includes(RelatedEntityInfo other)
		{
			Error.ArgumentNullException_IfNull(other, "other");

			return RelatedPropertyPath.StartsWith(other.RelatedPropertyPath);
		}

		/// <summary>
		/// Combines the two related entity information instances.
		/// </summary>
		/// <param name="firstInfo">The first related entity information.</param>
		/// <param name="secondInfo">The second related entity information.</param>
		/// <returns>The combined related entity information.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="firstInfo"/>
		/// or the <paramref name="secondInfo"/> is null.</exception>
		public static RelatedEntityInfo Combine(RelatedEntityInfo firstInfo, RelatedEntityInfo secondInfo)
		{
			Error.ArgumentNullException_IfNull(firstInfo, "firstInfo");
			Error.ArgumentNullException_IfNull(secondInfo, "secondInfo");

			if (firstInfo.IsEmpty())
			{
				return secondInfo;
			}
			else if (secondInfo.IsEmpty())
			{
				return firstInfo;
			}

			return new RelatedEntityInfo(string.Format("{0}.{1}", firstInfo.RelatedPropertyPath, secondInfo.RelatedPropertyPath),
				string.Format("{0}.{1}", firstInfo.RelatedKeyPath, secondInfo.RelatedKeyPath));
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the related property path.
		/// </summary>
		public string RelatedPropertyPath
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the related key path.
		/// </summary>
		public string RelatedKeyPath
		{
			get;
			private set;
		}

		#endregion

		#region Object Members

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			var other = obj as RelatedEntityInfo;
			return other != null && other.RelatedPropertyPath.Equals(RelatedPropertyPath)
				&& other.RelatedKeyPath.Equals(RelatedKeyPath);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return RelatedPropertyPath.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return RelatedPropertyPath;
		}

		#endregion
	}
}
