using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("Lookup")]
	[SpFieldConverter("LookupMulti")]
	internal class LookupFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

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
			if (value == null || string.IsNullOrEmpty(Field.LookupList))
				return null;

			if (!Field.AllowMultipleValues)
			{
				var fieldValue = new SPFieldLookupValue(value.ToString());

				return new ObjectReference
				{
					Id = fieldValue.LookupId,
					ListId = new Guid(Field.LookupList),
					Value = fieldValue.LookupValue
				};
			}

			var fieldValues = new SPFieldLookupValueCollection(value.ToString());

			return fieldValues.Select(fieldValue => new ObjectReference
				{
					Id = fieldValue.LookupId,
					ListId = new Guid(Field.LookupList),
					Value = fieldValue.LookupValue
				})
				.ToList();
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP field value.
		/// </summary>
		/// <param name="value">Member value to convert.</param>
		/// <returns>SP field value.</returns>
		public object ToSpValue(object value)
		{
			if (value == null)
				return null;
			
			if (!Field.AllowMultipleValues)
			{
				var reference = (ObjectReference)value;

				return new SPFieldLookupValue(reference.Id, reference.Value);
			}

			var references = (IEnumerable<ObjectReference>)value;

			var fieldValues = new SPFieldLookupValueCollection();
			fieldValues.AddRange(references.Select(referenceInfo => new SPFieldLookupValue(referenceInfo.Id, referenceInfo.Value)));

			return fieldValues;
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP Caml value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Caml value.</returns>
		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}
	}
}
