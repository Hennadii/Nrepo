namespace Nrepo
{
	/// <summary>
	/// Represents the base class for all the comparing conditions.
	/// </summary>
	public abstract class QueryFilterComparingCondition : QueryFilterCondition
	{
		#region Public Properties

		/// <summary>
		/// The ethalon value.
		/// </summary>
		public object Value
		{
			get;
			set;
		}

		#endregion
	}
}
