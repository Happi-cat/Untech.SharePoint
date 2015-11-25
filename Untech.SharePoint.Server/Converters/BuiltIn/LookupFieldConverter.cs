using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("Lookup")]
	[SpFieldConverter("LookupMulti")]
	[UsedImplicitly]
	internal class LookupFieldConverter : IFieldConverter
	{
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
			if (value == null || string.IsNullOrEmpty(Field.LookupList))
				return null;

			if (!Field.AllowMultipleValues)
			{
				var fieldValue = new SPFieldLookupValue(value.ToString());

				return FieldValueToObjectReference(fieldValue);
			}

			var fieldValues = new SPFieldLookupValueCollection(value.ToString());

			return fieldValues.Select(FieldValueToObjectReference)
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

				return ObjectReferenceToFieldValue(reference);
			}

			var references = (IEnumerable<ObjectReference>)value;

			var fieldValues = new SPFieldLookupValueCollection();
			fieldValues.AddRange(references.Select(ObjectReferenceToFieldValue));

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

		private ObjectReference FieldValueToObjectReference(SPFieldLookupValue fieldValue)
		{
			return new ObjectReference
			{
				Id = fieldValue.LookupId,
				ListId = new Guid(Field.LookupList),
				Value = fieldValue.LookupValue
			};
		}

		private static SPFieldLookupValue ObjectReferenceToFieldValue(ObjectReference referenceInfo)
		{
			return new SPFieldLookupValue(referenceInfo.Id, referenceInfo.Value);
		}
	}
}
