namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the parameters for getting entities.
	/// </summary>
	public class GetOperationParameters : OperationParameters
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the query parameters.
		/// </summary>
		public QueryParameters QueryParameters
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value indicating, whether to load sub entities or not.
		/// </summary>
		public bool LoadSubEntities
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value indicating, whether to load used entities or not.
		/// </summary>
		public bool LoadUsedEntities
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value indicating, whether to load using entities or not.
		/// </summary>
		public bool LoadUsingEntities
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
			return string.Format("{0}; LoadSubEntities:{1}; LoadUsedEntities:{2}; LoadUsingEntities:{3}; {4}",
				base.ToString(), LoadSubEntities, LoadUsedEntities, LoadUsingEntities,
				QueryParameters != null ? QueryParameters.ToString() : string.Empty);
		}

		#endregion
	}
}
