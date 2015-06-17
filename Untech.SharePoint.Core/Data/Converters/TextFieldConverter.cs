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
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(string))
				throw new ArgumentException("SPField with string value type only supported");

			if (propertyType != typeof(string))
				throw new ArgumentException("This converter can be used only with string property types");


			Field = field;
			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
	    {
		    return value == null ? null : value.ToString();
	    }

		public object ToSpValue(object value)
		{
			return value == null ? null : value.ToString();
		}
	}
}
