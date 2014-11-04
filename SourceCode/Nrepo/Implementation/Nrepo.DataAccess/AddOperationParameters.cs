using System;
namespace Nrepo.DataAccess
{
	/// <summary>
	/// Represents the parameters for adding entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity to add.</typeparam>
	public class AddOperationParameters<TEntity> : OperationParameters
		where TEntity : class
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the entity to add.
		/// </summary>
		public TEntity Entity
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
			return string.Format("{0}; EntityType:{1}", base.ToString(), typeof(TEntity));
		}

		#endregion
	}
}
