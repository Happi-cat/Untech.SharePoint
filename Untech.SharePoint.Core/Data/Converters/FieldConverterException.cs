using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}

	public class InvalidFieldConverterException : FieldConverterException
	{
		public InvalidFieldConverterException()
		{

		}
		public InvalidFieldConverterException(string message)
			: base(message)
		{

		}
		public InvalidFieldConverterException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}
