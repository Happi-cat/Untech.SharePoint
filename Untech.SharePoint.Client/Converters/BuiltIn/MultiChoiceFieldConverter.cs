using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
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

			if (field.MemberType != typeof (string[]) &&
			    !field.MemberType.IsAssignableFrom(typeof (List<string>)))
			{
				throw new ArgumentException(
					"Only string[] or any class assignable from List<string> can be used as a member type.");
			}

			IsArray = field.MemberType.IsArray;
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			var lookupValues = (IEnumerable<string>)value;

			return IsArray ? (object)lookupValues.ToArray() : lookupValues.ToList();
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			var lookupValues = (IEnumerable<string>)value;

			return lookupValues.ToList();
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return "";

			var singleValue = value as string;
			if (singleValue != null)
			{
				return string.Format(";#{0};#", singleValue);
			}

			var multiValue = ((IEnumerable<string>)value).ToList();
			return multiValue.Any() ? string.Format(";#{0};#", multiValue.JoinToString(";#")) : "";
		}
	}
}