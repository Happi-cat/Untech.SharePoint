using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents errors that occurs during <see cref="MetaField"/> mapping.
	/// </summary>
	public class DataMappingException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataMappingException"/> class with the specified <see cref="MetaField"/>.
		/// </summary>
		/// <param name="message">Message of the exception.</param>
		public DataMappingException(string message)
			: base(message)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataMappingException"/> class with the specified <see cref="MetaField"/>
		/// and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Message of the exception.</param>
		/// <param name="innerException">The exception that is the casue of this exception.</param>
		public DataMappingException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}