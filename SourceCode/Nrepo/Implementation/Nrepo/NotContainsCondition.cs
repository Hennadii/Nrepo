namespace Nrepo
{
	/// <summary>
	/// Represents the "not contains" filtering condition.
	/// </summary>
	public class NotContainsCondition : QueryFilterComparingCondition
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
			return string.Format("'{0}' NOT CONTAINS '{1}'",
				Property ?? string.Empty, Value != null ? Value.ToString() : string.Empty);
		}

		#endregion
	}
}
