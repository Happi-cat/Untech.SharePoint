using System;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	public class InvalidAnnotationException : Exception
	{
		public InvalidAnnotationException(string message)
			: base(message)
		{

		}
	}
}
