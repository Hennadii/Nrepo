using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nrepo.DataAccess.Internal;
using Nrepo.Internal;

namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the configuration object for related entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity to configure.</typeparam>
	public class RelatedEntityConfiguration<TEntity>
		where TEntity : class
	{
		#region Fields

		internal readonly IList<RelatedEntityInfo> subEntityInfo;

		internal readonly IList<RelatedEntityInfo> usedEntityInfo;

		internal readonly IList<RelatedEntityInfo> usingEntityInfo;

		private bool isSubEntityInfoOptimized;

		private bool isUsedEntityInfoOptimized;

		private bool isUsingEntityInfoOptimized;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the new instance of the <see cref="RelatedEntityConfiguration{TEntity}"/> class.
		/// </summary>
		/// <param name="subEntityInfo">Information about sub entities of the given entity.</param>
		/// <param name="usedEntityInfo">Information about used entities of the given entity.</param>
		/// <param name="usingEntityInfo">Information about using entities of the given entity.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="subEntityInfo"/> or
		/// <paramref name="usedEntityInfo"/> or <paramref name="usingEntityInfo"/> is null.</exception>
		internal RelatedEntityConfiguration(IList<RelatedEntityInfo> subEntityInfo,
			IList<RelatedEntityInfo> usedEntityInfo, IList<RelatedEntityInfo> usingEntityInfo)
		{
			Error.ArgumentNullException_IfNull(subEntityInfo, "subEntityInfo");
			Error.ArgumentNullException_IfNull(usedEntityInfo, "usedEntityInfo");
			Error.ArgumentNullException_IfNull(usingEntityInfo, "usingEntityInfo");

			this.subEntityInfo = subEntityInfo;
			this.usedEntityInfo = usedEntityInfo;
			this.usingEntityInfo = usingEntityInfo;

			isSubEntityInfoOptimized = false;
			isUsedEntityInfoOptimized = false;
			isUsingEntityInfoOptimized = false;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Configures a sub entity of the entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the sub entity property.</typeparam>
		/// <typeparam name="TKey">The type of the sub entity key.</typeparam>
		/// <param name="subEntityProperty">The sub entity property.</param>
		/// <param name="subEntityKey">The sub entity key.</param>
		/// <returns>The configuration object for related entities.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="subEntityProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="subEntityProperty"/> or
		/// <paramref name="subEntityKey"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> HasSubEntity<TProperty, TKey>(
			Expression<Func<TEntity, TProperty>> subEntityProperty, Expression<Func<TEntity, TKey>> subEntityKey)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(subEntityProperty, "subEntityProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(subEntityProperty);
			if (subEntityKey != null)
			{
				DAError.ArgumentException_IfNotOneLevelPropertyExpression(subEntityKey);
			}

			var member = new RelatedEntityInfo(ReflectionHelper.GetPropertyName(subEntityProperty),
				subEntityKey != null ? ReflectionHelper.GetPropertyName(subEntityKey) : string.Empty);

			if (!subEntityInfo.Contains(member))
			{
				subEntityInfo.Add(member);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(
				subEntityInfo, member, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		/// <summary>
		/// Configures sub entities of the entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the sub entities property.</typeparam>
		/// <param name="subEntitiesProperty">The sub collection.</param>
		/// <returns>The configuration object to configure sub entities.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="subEntitiesProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="subEntitiesProperty"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> HasSubEntities<TProperty>(
			Expression<Func<TEntity, ICollection<TProperty>>> subEntitiesProperty)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(subEntitiesProperty, "subEntitiesProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(subEntitiesProperty);

			var member = new RelatedEntityInfo(ReflectionHelper.GetPropertyName(subEntitiesProperty), string.Empty);

			if (!subEntityInfo.Contains(member))
			{
				subEntityInfo.Add(member);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(
				subEntityInfo, member, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		/// <summary>
		/// Configures a used entity for the entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the used entity property.</typeparam>
		/// <typeparam name="TKey">The type of the used entity key.</typeparam>
		/// <param name="usedEntityProperty">The used entity property.</param>
		/// <param name="usedEntityKey">The used entity key.</param>
		/// <returns>The configuration object for related entities.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="usedEntityProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="usedEntityProperty"/>
		/// or <paramref name="usedEntityKey"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> UsesEntity<TProperty, TKey>(
			Expression<Func<TEntity, TProperty>> usedEntityProperty,
			Expression<Func<TEntity, TKey>> usedEntityKey)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(usedEntityProperty, "usedEntityProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(usedEntityProperty);
			if (usedEntityKey != null)
			{
				DAError.ArgumentException_IfNotOneLevelPropertyExpression(usedEntityKey);
			}

			var member = new RelatedEntityInfo(ReflectionHelper.GetPropertyName(usedEntityProperty),
				usedEntityKey != null ? ReflectionHelper.GetPropertyName(usedEntityKey) : string.Empty);

			if (!usedEntityInfo.Contains(member))
			{
				usedEntityInfo.Add(member);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(usedEntityInfo,
				member, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		/// <summary>
		/// Configures used entities for the entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the used entities property.</typeparam>
		/// <param name="usedEntitiesProperty">The used collection.</param>
		/// <returns>The configuration object to configure a used entity item.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="usedEntitiesProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="usedEntitiesProperty"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> UsesEntities<TProperty>(
			Expression<Func<TEntity, ICollection<TProperty>>> usedEntitiesProperty)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(usedEntitiesProperty, "usedEntitiesProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(usedEntitiesProperty);

			var member = new RelatedEntityInfo(ReflectionHelper.GetPropertyName(usedEntitiesProperty), string.Empty);

			if (!usedEntityInfo.Contains(member))
			{
				usedEntityInfo.Add(member);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(usedEntityInfo,
				member, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		/// <summary>
		/// Configures a using entity for the entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the using entity property.</typeparam>
		/// <typeparam name="TKey">The type of the using entity key.</typeparam>
		/// <param name="usingEntityProperty">The using entity property.</param>
		/// <param name="usingEntityKey">The using entity key.</param>
		/// <returns>The configuration object for related entities.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="usingEntityProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="usingEntityProperty"/>
		/// or <paramref name="usingEntityKey"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> IsUsedByEntity<TProperty, TKey>(
			Expression<Func<TEntity, TProperty>> usingEntityProperty,
			Expression<Func<TEntity, TKey>> usingEntityKey)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(usingEntityProperty, "usingEntityProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(usingEntityProperty);
			if (usingEntityKey != null)
			{
				DAError.ArgumentException_IfNotOneLevelPropertyExpression(usingEntityKey);
			}

			var member = new RelatedEntityInfo(ReflectionHelper.GetPropertyName(usingEntityProperty),
				usingEntityKey != null ? ReflectionHelper.GetPropertyName(usingEntityKey) : string.Empty);

			if (!usingEntityInfo.Contains(member))
			{
				usingEntityInfo.Add(member);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(
				usingEntityInfo, member, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		/// <summary>
		/// Configures using entities for the entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the using entities property.</typeparam>
		/// <param name="usingEntitiesProperty">The using collection.</param>
		/// <returns>The configuration object to configure a using entities.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="usingEntitiesProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="usingEntitiesProperty"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> IsUsedByEntities<TProperty>(
			Expression<Func<TEntity, ICollection<TProperty>>> usingEntitiesProperty)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(usingEntitiesProperty, "usingEntitiesProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(usingEntitiesProperty);

			var member = new RelatedEntityInfo(ReflectionHelper.GetPropertyName(usingEntitiesProperty), string.Empty);

			if (!usingEntityInfo.Contains(member))
			{
				usingEntityInfo.Add(member);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(
				usingEntityInfo, member, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		/// <summary>
		/// Gets information about sub entities.
		/// </summary>
		/// <returns>Information about sub entities..</returns>
		public RelatedEntityInfo[] GetSubEntityInfo()
		{
			if (!isSubEntityInfoOptimized)
			{
				OptimizeRelatedEntityInfo(subEntityInfo);

				isSubEntityInfoOptimized = true;
			}

			return subEntityInfo.ToArray();
		}

		/// <summary>
		/// Gets information about used entities.
		/// </summary>
		/// <returns>Information about used entities.</returns>
		public RelatedEntityInfo[] GetUsedEntityInfo()
		{
			if (!isUsedEntityInfoOptimized)
			{
				OptimizeRelatedEntityInfo(usedEntityInfo);

				isUsedEntityInfoOptimized = true;
			}

			return usedEntityInfo.ToArray();
		}

		/// <summary>
		/// Gets information about using entities.
		/// </summary>
		/// <returns>Information about using entities.</returns>
		public RelatedEntityInfo[] GetUsingEntityInfo()
		{
			if (!isUsingEntityInfoOptimized)
			{
				OptimizeRelatedEntityInfo(usingEntityInfo);

				isUsingEntityInfoOptimized = true;
			}

			return usingEntityInfo.ToArray();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Removes extra related entity configurations.
		/// </summary>
		/// <param name="relatedEntityInfo">Information about related entities to optimize.</param>
		/// <example>
		/// The list below:
		/// 
		/// [0].RelatedPropertyPath User.Role.Permissions
		/// [1].RelatedPropertyPath User.Role
		/// 
		/// can be reduced to:
		/// 
		/// [0].RelatedPropertyPath User.Role.Permissions
		/// </example>
		private void OptimizeRelatedEntityInfo(IList<RelatedEntityInfo> relatedEntityInfo)
		{
			var remove = new List<RelatedEntityInfo>(relatedEntityInfo.Count);

			for (int i = 0; i < relatedEntityInfo.Count; i++)
			{
				for (int j = 0; j < relatedEntityInfo.Count; j++)
				{
					if (relatedEntityInfo[i].RelatedPropertyPath.StartsWith(relatedEntityInfo[j].RelatedPropertyPath) && i != j)
					{
						remove.Add(relatedEntityInfo[j]);
					}
				}
			}

			for (int i = 0; i < remove.Count; i++)
			{
				relatedEntityInfo.Remove(remove[i]);
			}
		}

		#endregion
	}
}
