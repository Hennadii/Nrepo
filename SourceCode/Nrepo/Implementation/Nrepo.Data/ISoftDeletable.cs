namespace Nrepo.Data
{
	/// <summary>
	/// The base interface for entities with soft deletion.
	/// </summary>
	public interface ISoftDeletable : IEditable
	{
		#region Properties

		/// <summary>
		/// Indicates whether this entity is deleted or not.
		/// </summary>
		bool IsDeleted
		{
			get;
			set;
		}

		#endregion
	}
}
