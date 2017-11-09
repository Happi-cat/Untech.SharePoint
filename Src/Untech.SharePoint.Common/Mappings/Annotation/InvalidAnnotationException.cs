using System;

namespace Untech.SharePoint.Mappings.Annotation
{
	/// <summary>
	/// Represents errors that occurs due to invalid or incomplete annotation. 
	/// </summary>
	[Serializable]
	public class InvalidAnnotationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidAnnotationException"/> class.
		/// </summary>
		/// <param name="message">Exception message</param>
		public InvalidAnnotationException(string message)
			: base(message)
		{
		}
	}
}
