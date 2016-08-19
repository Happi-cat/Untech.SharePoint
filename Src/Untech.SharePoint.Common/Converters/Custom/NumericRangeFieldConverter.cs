using System;
using System.Globalization;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.Custom
{
	/// <summary>
	/// Represetns field convertert that can convert string to <see cref="Tuple{Double,Double}"/> and vice versa.
	/// This converter use next notation for string: 0.0..1.0
	/// </summary>
	[SpFieldConverter("_NumericRange_")]
	public class NumericRangeFieldConverter : IFieldConverter
	{
		public void Initialize(MetaField field)
		{
			if (!field.MemberType.IsAssignableFrom(typeof(Tuple<double, double>)))
			{
				throw new ArgumentException("Member type should be assignable from System.Tuple<double, double>.");
			}
		}

		public object FromSpValue(object value)
		{
			var stringValue = (string)value ?? "..";

			double min = 0;
			double max = 0;
			var delimeterIndex = stringValue.IndexOf("..", StringComparison.Ordinal);

			if (delimeterIndex > -1)
			{
				double.TryParse(stringValue.Substring(0, delimeterIndex), NumberStyles.Any, CultureInfo.InvariantCulture, out min);
				double.TryParse(stringValue.Substring(delimeterIndex + 2), NumberStyles.Any, CultureInfo.InvariantCulture, out max);
			}

			return new Tuple<double, double>(min, max);
		}

		public object ToSpValue(object value)
		{
			var fieldValue = (Tuple<double, double>) value;

			if (fieldValue == null) return null;

			return string.Format("{0}..{1}",
				fieldValue.Item1.ToString("F2", CultureInfo.InvariantCulture),
				fieldValue.Item2.ToString("F2", CultureInfo.InvariantCulture));
		}

		public string ToCamlValue(object value)
		{
			return (string) ToSpValue(value) ?? "";
		}
	}
}