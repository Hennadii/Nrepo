using System.Reflection;

namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the context for an action with a related entity.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public class RelatedEntityActionContext<TEntity>
		where TEntity : class
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the root entity.
		/// </summary>
		public TEntity RootEntity
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the related entity.
		/// </summary>
		public object RelatedEntity
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the related entity container.
		/// </summary>
		public object RelatedEntityContainer
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the related entity information.
		/// </summary>
		public RelatedEntityInfo RelatedEntityInfo
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the related entity property information.
		/// </summary>
		public PropertyInfo RelatedEntityPropertyInfo
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the related entity key information.
		/// </summary>
		public PropertyInfo RelatedEntityKeyInfo
		{
			get;
			set;
		}

		#endregion
	}
}
