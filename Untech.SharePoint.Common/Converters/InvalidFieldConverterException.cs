using System;

namespace Untech.SharePoint.Common.Converters
{
	public class InvalidFieldConverterException : FieldConverterException
	{
		public InvalidFieldConverterException(Type converterType)
			: base(GetMessage(converterType))
		{

		}

		public InvalidFieldConverterException(Type converterType, Exception innerException)
			: base(GetMessage(converterType), innerException)
		{

		}

		private static string GetMessage(Type converterType)
		{
			return string.Format("Invalid Field converter '{0}'", converterType);
		}
	}
}