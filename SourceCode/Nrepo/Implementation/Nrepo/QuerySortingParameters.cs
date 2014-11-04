namespace Nrepo
{
	/// <summary>
	/// Represents the sorting parameters.
	/// </summary>
	public class QuerySortingParameters
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the sorting type.
		/// </summary>
		public SortingType SortingType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name of the property of the entity to sort by.
		/// </summary>
		public string SortBy
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
			return string.Format("SortingType:{0}; SortBy:{1}", SortingType, SortBy);
		}

		#endregion
	}
}
