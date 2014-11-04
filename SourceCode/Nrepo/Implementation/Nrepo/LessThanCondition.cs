namespace Nrepo
{
	/// <summary>
	/// Represents the "less than" filtering condition.
	/// </summary>
	public class LessThanCondition : QueryFilterComparingCondition
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
			return string.Format("'{0}' LESS THAN '{1}'",
				Property ?? string.Empty, Value != null ? Value.ToString() : string.Empty);
		}

		#endregion
	}
}
