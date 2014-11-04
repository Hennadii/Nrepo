﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Nrepo
{
	/// <summary>
	/// Represents the base class for all the exceptions of the framework.
	/// </summary>
	[Serializable]
	public class NrepoException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NrepoException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
		public NrepoException(string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NrepoException"/> class.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		[SecurityPermission(SecurityAction.LinkDemand,
			Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected NrepoException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
