using System;

namespace Nrepo.Data
{
	/// <summary>
	/// The interface for entities with auditable creation.
	/// </summary>
	public interface ICreateAuditable : IEditable
	{
		#region Properties

		/// <summary>
		/// Gets or sets the owner's id, which created the entity.
		/// </summary>
		long CreatedById
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the time when this entity was created.
		/// </summary>
		DateTime CreatedOn
		{
			get;
			set;
		}

		#endregion
	}
}
