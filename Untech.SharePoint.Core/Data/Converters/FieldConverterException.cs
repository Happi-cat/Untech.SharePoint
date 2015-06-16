using System;

namespace Untech.SharePoint.Core.Data.Converters
{
	public class FieldConverterException : Exception
	{
		public FieldConverterException()
		{

		}
		public FieldConverterException(string message)
			: base(message)
		{

		}
		public FieldConverterException(string message, Exception innerException)
			: base(message, innerException)
		{

		}

		public FieldConverterException(Type converterType, Exception innerException)
			: base(GetMessage(converterType), innerException)
		{
			
		}

		private static string GetMessage(Type converterType)
		{
			return string.Format("Occured error with {0} field converter", converterType.FullName);
		}
	}
}
