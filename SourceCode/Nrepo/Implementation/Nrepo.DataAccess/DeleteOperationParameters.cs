namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the parameters for deleting entities.
	/// </summary>
	public class DeleteOperationParameters : OperationParameters
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the primary keys of the entity to delete.
		/// </summary>
		public object[] PrimaryKeys
		{
			get;
			set;
		}

		#endregion

		#region Object Members

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0}; PrimaryKeys:{1}", base.ToString(),
				PrimaryKeys != null ? string.Join(",", PrimaryKeys) : string.Empty);
		}

		#endregion
	}
}
