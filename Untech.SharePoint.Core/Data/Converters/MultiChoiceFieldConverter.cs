using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("MultiChoice")]
	public class MultiChoiceFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			var multiChoiceField = field as SPFieldMultiChoice;
			if (multiChoiceField == null)
			{
				throw new ArgumentException();
			}

			if (value == null)
				return null;

			return GetValues(new SPFieldMultiChoiceValue(value.ToString()));
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			if (value == null)
				return null;

			var strings = ((IEnumerable<string>) value).ToList();

			var fieldValues = new SPFieldMultiChoiceValue();
			foreach (var s in strings)
			{
				fieldValues.Add(s);
			}
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
