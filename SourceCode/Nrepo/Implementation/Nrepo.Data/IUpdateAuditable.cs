using System;

namespace Nrepo.Data
{
	/// <summary>
	/// The base interface for entities with auditable updating.
	/// </summary>
	public interface IUpdateAuditable : IEditable
	{
		#region Properties

		/// <summary>
		/// Gets or sets the last owner's id, which updated the entity.
		/// </summary>
		long LastUpdatedById
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the last time when this entity was updated.
		/// </summary>
		DateTime LastUpdateOn
		{
			get;
			set;
		}

		#endregion
	}
}
