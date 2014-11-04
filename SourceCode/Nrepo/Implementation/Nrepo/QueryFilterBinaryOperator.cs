namespace Nrepo
{
	/// <summary>
	/// Represents the base class for all the binary filtering operators.
	/// </summary>
	public abstract class QueryFilterBinaryOperator : QueryFilterOperator
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the left operand.
		/// </summary>
		public QueryFilterExpression LeftOperand
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the right operand.
		/// </summary>
		public QueryFilterExpression RightOperand
		{
			get;
			set;
		}

		#endregion
	}
}
