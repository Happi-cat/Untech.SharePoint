using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.Custom
{
	public class KeyValueFieldConverter : IFieldConverter
	{
		private const string PairDelimiter = ";";
		private const string KeyValueDelimiter = ":";

		private MetaField Field { get; set; }

		/// <summary>
		/// Initialzes current instance with the specified <see cref="MetaField"/>
		/// </summary>
		/// <param name="field"></param>
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		/// <summary>
		/// Converts SP field value to <see cref="MetaField.MemberType"/>.
		/// </summary>
		/// <param name="value">SP value to convert.</param>
		/// <returns>Member value.</returns>
		public object FromSpValue(object value)
		{
			if (value == null) return null;
			var collection = new Dictionary<string, string>();

			((string) value)
				.Split(new[] {PairDelimiter}, StringSplitOptions.RemoveEmptyEntries)
				.Select(SplitKeyValue)
				.Where(n => n.Length > 0)
				.Each(n => collection.Add(n[0], n.ElementAtOrDefault(1)));

			return collection;
		}

		private static string[] SplitKeyValue(string str)
		{
			return str.Split(new[] {KeyValueDelimiter}, StringSplitOptions.RemoveEmptyEntries)
				.Select(n => n.Trim())
				.ToArray();
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP field value.
		/// </summary>
		/// <param name="value">Member value to convert.</param>
		/// <returns>SP field value.</returns>
		public object ToSpValue(object value)
		{
			if (value == null) return null;
			var collection = (IEnumerable<KeyValuePair<string, string>>) value;

			return collection
				.Select(n => string.Format("{0}{1}{2}", n.Key, KeyValueDelimiter, n.Value))
				.JoinToString(PairDelimiter);
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP Caml value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Caml value.</returns>
		public string ToCamlValue(object value)
		{
			return (string) ToSpValue(value) ?? "";
		}

		
	}
}