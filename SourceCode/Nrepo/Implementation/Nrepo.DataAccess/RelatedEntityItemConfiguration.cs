using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nrepo.DataAccess.Internal;
using Nrepo.Internal;

namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the configuration object for items of a related entity.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	/// <typeparam name="TItem">The type of the related entity item.</typeparam>
	public sealed class RelatedEntityItemConfiguration<TEntity, TItem>
		: RelatedEntityConfiguration<TEntity>
		where TEntity : class
		where TItem : class
	{
		#region Fields

		private readonly string newMemberFormat2 = "{0}.{1}";

		private RelatedEntityInfo currentRelatedEntityInfo;

		private readonly IList<RelatedEntityInfo> relatedEntityInfo;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the new instance of the <see cref="RelatedEntityItemConfiguration{TEntity,TItem}"/> class.
		/// </summary>
		/// <param name="relatedEntityInfo">Information about related entities.</param>
		/// <param name="currentRelatedEntityInfo">Information about the current related entity to be continued.</param>
		/// <param name="subEntityInfo">Information about sub entities for the given entity.</param>
		/// <param name="usedEntityInfo">Information about entities used by the given entity.</param>
		/// <param name="usingEntityInfo">Information about entities using the given entity.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="relatedEntityInfo"/> or
		/// <paramref name="currentRelatedEntityInfo"/> or <paramref name="subEntityInfo"/> or
		/// <paramref name="usedEntityInfo"/> or <paramref name="usingEntityInfo"/> is null.</exception>
		internal RelatedEntityItemConfiguration(IList<RelatedEntityInfo> relatedEntityInfo,
			RelatedEntityInfo currentRelatedEntityInfo, IList<RelatedEntityInfo> subEntityInfo,
			IList<RelatedEntityInfo> usedEntityInfo, IList<RelatedEntityInfo> usingEntityInfo)
			: base(subEntityInfo, usedEntityInfo, usingEntityInfo)
		{
			Error.ArgumentNullException_IfNull(relatedEntityInfo, "currentEntityInfo");
			Error.ArgumentNullException_IfNull(currentRelatedEntityInfo, "currentRelatedEntityInfo");

			this.relatedEntityInfo = relatedEntityInfo;
			this.currentRelatedEntityInfo = currentRelatedEntityInfo;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Configures a related entity of the given entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the related entity property.</typeparam>
		/// <typeparam name="TKey">The type of the related entity key.</typeparam>
		/// <param name="relatedEntityProperty">The related entity property.</param>
		/// <param name="relatedEntityKey">The related entity key.</param>
		/// <returns>The configuration ovject of the given entity.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="relatedEntityProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="relatedEntityProperty"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> WithEntity<TProperty, TKey>(
			Expression<Func<TItem, TProperty>> relatedEntityProperty,
			Expression<Func<TItem, TKey>> relatedEntityKey)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(relatedEntityProperty, "relatedEntityProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(relatedEntityProperty);
			if (relatedEntityKey != null)
			{
				DAError.ArgumentException_IfNotOneLevelPropertyExpression(relatedEntityKey);
			}

			var memberProperty = ReflectionHelper.GetPropertyName(relatedEntityProperty);
			var memberKey = relatedEntityKey != null ? ReflectionHelper.GetPropertyName(relatedEntityKey) : string.Empty;

			var newMember = new RelatedEntityInfo(
				string.Format(newMemberFormat2, currentRelatedEntityInfo.RelatedPropertyPath, memberProperty),
				string.Format(newMemberFormat2, currentRelatedEntityInfo.RelatedKeyPath, memberKey));

			if (!relatedEntityInfo.Contains(newMember))
			{
				relatedEntityInfo.Remove(currentRelatedEntityInfo);

				relatedEntityInfo.Add(newMember);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(
				relatedEntityInfo, newMember, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		/// <summary>
		/// Configures related entities of the given entity.
		/// </summary>
		/// <typeparam name="TProperty">The type of the item of the related entities list property.</typeparam>
		/// <param name="relatedEntitiesProperty">The related entities.</param>
		/// <returns>The configuration object to configure related entities.</returns>
		/// <exception cref="System.ArgumentNullException">The <paramref name="relatedEntitiesProperty"/> is null.</exception>
		/// <exception cref="System.ArgumentException">The <paramref name="relatedEntitiesProperty"/> is not an one level property expression.</exception>
		public RelatedEntityItemConfiguration<TEntity, TProperty> WithEntities<TProperty>(
			Expression<Func<TItem, ICollection<TProperty>>> relatedEntitiesProperty)
			where TProperty : class
		{
			Error.ArgumentNullException_IfNull(relatedEntitiesProperty, "relatedEntitiesProperty");
			DAError.ArgumentException_IfNotOneLevelPropertyExpression(relatedEntitiesProperty);

			var memberProperty = ReflectionHelper.GetPropertyName(relatedEntitiesProperty);

			var newMember = new RelatedEntityInfo(
				string.Format(newMemberFormat2, currentRelatedEntityInfo.RelatedPropertyPath, memberProperty),
				string.Format(newMemberFormat2, currentRelatedEntityInfo.RelatedKeyPath, string.Empty));

			if (!relatedEntityInfo.Contains(newMember))
			{
				relatedEntityInfo.Remove(currentRelatedEntityInfo);

				relatedEntityInfo.Add(newMember);
			}

			return new RelatedEntityItemConfiguration<TEntity, TProperty>(
				relatedEntityInfo, newMember, subEntityInfo, usedEntityInfo, usingEntityInfo);
		}

		#endregion
	}
}
