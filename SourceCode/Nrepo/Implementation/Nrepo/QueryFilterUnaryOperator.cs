namespace Nrepo
{
	/// <summary>
	/// Represents the base class for all the unary filtering operators.
	/// </summary>
	public abstract class QueryFilterUnaryOperator : QueryFilterOperator
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the filtering operand to filter with this operator.
		/// </summary>
		public QueryFilterExpression Operand
		{
			get;
			set;
		}

		#endregion
	}
}
