using System;

namespace Untech.SharePoint.Core.Data.Converters
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
			return string.Format("Field converter {0} is invalid", converterType.FullName);
		}
	}
}