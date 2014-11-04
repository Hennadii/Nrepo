namespace Nrepo
{
	/// <summary>
	/// Represents query parameters.
	/// </summary>
	public class QueryParameters
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets sorting parameters.
		/// </summary>
		public QuerySortingParameters SortingParameters
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets paging parameters.
		/// </summary>
		public QueryPagingParameters PagingParameters
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the filtering expression.
		/// </summary>
		public QueryFilterExpression Filter
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
			return string.Format("{0}; {1}; {2}",
				SortingParameters != null ? SortingParameters.ToString() : string.Empty,
				PagingParameters != null ? PagingParameters.ToString() : string.Empty,
				Filter != null ? Filter.ToString() : string.Empty);
		}

		#endregion
	}
}
