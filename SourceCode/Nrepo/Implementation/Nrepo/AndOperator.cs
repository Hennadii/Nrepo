namespace Nrepo
{
	/// <summary>
	/// Represents the "and" operator.
	/// </summary>
	public class AndOperator : QueryFilterBinaryOperator
	{
		#region Object Members

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("({0}) AND ({1})",
				LeftOperand != null ? LeftOperand.ToString() : string.Empty,
				RightOperand != null ? RightOperand.ToString() : string.Empty);
		}

		#endregion
	}
}
