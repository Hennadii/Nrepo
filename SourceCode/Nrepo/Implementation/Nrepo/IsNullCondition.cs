namespace Nrepo
{
	/// <summary>
	/// Represents the "is null" filtering condition.
	/// </summary>
	public class IsNullCondition : QueryFilterCondition
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
			return string.Format("'{0}' IS NULL", Property ?? string.Empty);
		}

		#endregion
	}
}
