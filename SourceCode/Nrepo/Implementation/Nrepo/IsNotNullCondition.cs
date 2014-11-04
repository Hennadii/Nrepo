namespace Nrepo
{
	/// <summary>
	/// Represents the "is not null" filtering condition.
	/// </summary>
	public class IsNotNullCondition : QueryFilterCondition
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
			return string.Format("'{0}' IS NOT NULL", Property ?? string.Empty);
		}

		#endregion
	}
}
