namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the parameters for updating entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity to update.</typeparam>
	public class UpdateOperationParameters<TEntity> : OperationParameters
		where TEntity : class
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the entity to update.
		/// </summary>
		public TEntity Entity
		{
			get;
			set;
		}

		#endregion
	}
}
