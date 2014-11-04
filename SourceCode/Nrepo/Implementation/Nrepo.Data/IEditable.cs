namespace Nrepo.Data
{
	/// <summary>
	/// The base interface for editable entities.
	/// </summary>
	public interface IEditable
	{
		#region Properties

		/// <summary>
		/// Indicates whether this entity is used or not. Is set automatically by the repository,
		/// when the entity is loaded with using entities.
		/// </summary>
		/// <returns><c>true</c> if entity is used, otherwise <c>false</c>.</returns>
		bool IsUsed
		{
			get;
			set;
		}

		#endregion
	}
}
