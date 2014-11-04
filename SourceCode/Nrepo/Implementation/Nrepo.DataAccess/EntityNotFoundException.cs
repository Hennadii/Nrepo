﻿using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Nrepo.DataAccess
{
	/// <summary>
	/// The exception that is thrown when an entity is not found.
	/// </summary>
	[Serializable]
	public sealed class EntityNotFoundException : NrepoDataException
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="primaryKeys">The primary keys.</param>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public EntityNotFoundException(Type entityType, object[] primaryKeys, string message = null,
			Exception innerException = null)
			: base(message, innerException)
		{
			EntityType = entityType;
			PrimaryKeys = primaryKeys;
		}

		/// <summary>
		/// Prevents a default instance of the <see cref="EntityNotFoundException"/> class from being created.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		[SecurityPermission(SecurityAction.LinkDemand,
			Flags = SecurityPermissionFlag.SerializationFormatter)]
		private EntityNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			EntityType = (Type)info.GetValue("EntityType", typeof(Type));
			PrimaryKeys = (object[])info.GetValue("PrimaryKeys", typeof(object[]));
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		public Type EntityType
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the primary keys.
		/// </summary>
		public object[] PrimaryKeys
		{
			get;
			private set;
		}

		#endregion

		#region ISerializable Members

		/// <summary>
		/// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
		///   </PermissionSet>
		[SecurityPermission(SecurityAction.LinkDemand,
		Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("EntityType", EntityType);
			info.AddValue("PrimaryKeys", PrimaryKeys);

			base.GetObjectData(info, context);
		}

		#endregion

		#region Exception Members

		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
		public override string Message
		{
			get
			{
				return string.Format("{0}. {1}. {2}.", base.Message,
					EntityType != null ? EntityType.FullName : string.Empty,
					PrimaryKeys != null ? string.Join(",", PrimaryKeys).TrimEnd(',') : string.Empty);
			}
		}

		#endregion

		#region Object Members

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj)
		{
			var other = obj as EntityNotFoundException;

			if (other == null)
			{
				return false;
			}

			return base.Equals(obj) && EntityType == other.EntityType
				&& ((PrimaryKeys == null && other.PrimaryKeys == null)
				|| (PrimaryKeys.All(e => other.PrimaryKeys.Any(s => e == s))));
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
