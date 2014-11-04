using System;

namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the base class for all the repository operations.
	/// </summary>
	public class OperationParameters
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the owner of the operation.
		/// </summary>
		public long OwnerId
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the operation date time.
		/// </summary>
		public DateTime OperationDateTime
		{
			get;
			set;
		}

		#endregion

		#region Object Members

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("OwnerId:{0}; OperationDateTime:{1}", OwnerId, OperationDateTime);
		}

		#endregion
	}
}
