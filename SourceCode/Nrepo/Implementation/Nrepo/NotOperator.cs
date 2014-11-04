namespace Nrepo
{
	/// <summary>
	/// Represents the "not" operator.
	/// </summary>
	public class NotOperator : QueryFilterUnaryOperator
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
			return string.Format("NOT ()", Operand != null ? Operand.ToString() : string.Empty);
		}

		#endregion
	}
}
