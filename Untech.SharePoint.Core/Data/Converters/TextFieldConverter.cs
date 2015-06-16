using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Text")]
	[SPFieldConverter("Note")]
	[SPFieldConverter("Choice")]
	internal class TextFieldConverter:IFieldConverter
	{
	    public object FromSpValue(object value, SPField field, Type propertyType)
		{
			if (value == null)
				return null;

			if(!(field is SPFieldText) && !(field is SPFieldChoice))
			{
				throw new ArgumentException();
			}

			return value.ToString();
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			if (value == null)
				return null;

			if (!(field is SPFieldText))
			{
				throw new ArgumentException();
			}

			return value.ToString();
		}
	}
}
