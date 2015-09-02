using System;

namespace Untech.SharePoint.Common.Converters
{
	public class FieldConverterInitializationException : FieldConverterException
	{
		public FieldConverterInitializationException(Type converterType)
			: base(GetMessage(converterType))
		{

		}

		public FieldConverterInitializationException(Type converterType, Exception innerException)
			: base(GetMessage(converterType), innerException)
		{

		}

		private static string GetMessage(Type converterType)
		{
			return string.Format("Field converter '{0}' cannot be initialized", converterType);
		}
	}
}