using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("MultiChoice")]
	internal class MultiChoiceFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.NotNull(field, "field");
			Guard.NotNull(propertyType, "propertyType");

			Guard.TypeIs<SPFieldMultiChoiceValue>(field.FieldValueType, "field.FieldValueType");

			Guard.ArrayOrAssignableFromList<string>(propertyType, "propertType");

			
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			var values = GetValues(new SPFieldMultiChoiceValue(value.ToString()));
			return PropertyType == typeof (string[]) ? values : values.ToList();
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var strings = ((IEnumerable<string>) value).ToList();

			var fieldValues = new SPFieldMultiChoiceValue();
			
			strings.ForEach(s => fieldValues.Add(s));
			
			return fieldValues;
		}

		private IEnumerable<string> GetValues(SPFieldMultiChoiceValue value)
		{
			var strings = new string[value.Count];
			for (int i = 0; i < value.Count; i++)
			{
				strings[i] = value[i];
			}
			return strings;
		}

	}
}
