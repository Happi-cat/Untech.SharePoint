using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Text")]
	[SPFieldConverter("Note")]
	[SPFieldConverter("Choice")]
	internal class TextFieldConverter:IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Field = field;
			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			if(!(Field is SPFieldText) && !(Field is SPFieldChoice))
			{
				throw new ArgumentException();
			}

			return value.ToString();
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			if (!(Field is SPFieldText))
			{
				throw new ArgumentException();
			}

			return value.ToString();
		}
	}
}
