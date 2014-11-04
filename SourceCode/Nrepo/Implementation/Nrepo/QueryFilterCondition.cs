namespace Nrepo
{
	/// <summary>
	/// Represents the base class for all the filtering conditions.
	/// </summary>
	public abstract class QueryFilterCondition : QueryFilterExpression
	{
		#region Public Properties

		/// <summary>
		/// The property to be filtered by.
		/// </summary>
		public string Property
		{
			get;
			set;
		}

		#endregion
	}
}
