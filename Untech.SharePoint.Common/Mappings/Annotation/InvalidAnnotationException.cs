using System;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	public class InvalidAnnotationException : Exception
	{
		public InvalidAnnotationException(string message)
			: base(message)
		{

		}
	}
}
