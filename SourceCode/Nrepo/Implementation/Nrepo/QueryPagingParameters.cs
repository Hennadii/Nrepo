namespace Nrepo
{
	/// <summary>
	/// Represents the paging parameters.
	/// </summary>
	public class QueryPagingParameters
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets a zero-based page number to load entities from a database.
		/// </summary>
		public int PageNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the page size to load from a database.
		/// </summary>
		public int PageSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the total entities count found by these query parameters.
		/// </summary>
		public int TotalCount
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
			return string.Format("PageNumber:{0}; PageSize:{1}; TotalCount:{2}",
				PageNumber, PageSize, TotalCount);
		}

		#endregion
	}
}
