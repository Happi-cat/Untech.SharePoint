using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("MultiChoice")]
	public class MultiChoiceFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(SPFieldMultiChoiceValue))
				throw new ArgumentException("SPField with SPFieldMultiChoiceValue value type only supported");

			if (!propertyType.IsAssignableFrom(typeof(List<string>)) && propertyType != typeof(string[]))
				throw new ArgumentException("This converter can be used only with string[] or with types assignable from List<string>");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			var multiChoiceField = Field as SPFieldMultiChoice;
			if (multiChoiceField == null)
			{
				throw new ArgumentException();
			}

			if (value == null)
				return null;

			return GetValues(new SPFieldMultiChoiceValue(value.ToString()));
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
