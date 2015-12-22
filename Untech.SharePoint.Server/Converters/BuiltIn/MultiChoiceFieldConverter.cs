using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("MultiChoice")]
	[UsedImplicitly]
	internal class MultiChoiceFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }

		private bool IsArray { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;

			if (field.MemberType != typeof(string[]) &&
			    !field.MemberType.IsAssignableFrom(typeof(List<string>)))
			{
				throw new ArgumentException(
					"Only string[] or any class assignable from List<string> can be used as a member type.");
			}

			IsArray = field.MemberType.IsArray;
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			var fieldValues = new SPFieldMultiChoiceValue(value.ToString());
			var lookupValues = new List<string>();

			for (int i = 0; i < fieldValues.Count; i++)
			{
				lookupValues.Add(fieldValues[i]);
			}

			return IsArray ? (object)lookupValues.ToArray() : lookupValues;
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;


			var lookupValues = (IEnumerable<string>)value;
			var fieldValues = new SPFieldMultiChoiceValue();
			lookupValues.Each(n => fieldValues.Add(n));

			return fieldValues.ToString();
		}

		public string ToCamlValue(object value)
		{
			// TODO
			if (value == null) return null;

			var singleValue = value as string;
			if (singleValue != null)
			{
				return singleValue;
			}

			var multiValue = (IEnumerable<string>)value;
			return multiValue.JoinToString(";#");
		}
	}
}