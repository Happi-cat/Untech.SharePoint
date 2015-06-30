using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SpFieldConverter("Lookup")]
	[SpFieldConverter("LookupMulti")]
	internal class LookupFieldConverter : IFieldConverter
	{
		public SPFieldLookup Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.ThrowIfArgumentNull(field, "field");
			Guard.ThrowIfArgumentNull(propertyType, "propertyType");

			Field = field as SPFieldLookup;
			if (Field == null) throw new ArgumentException("SPFieldLookup is only supported", "field");

			if (Field.AllowMultipleValues)
			{
				Guard.ThrowIfArgumentNotArrayOrAssignableFromList<ObjectReference>(propertyType, "propertType");
			}
			else
			{
				Guard.ThrowIfArgumentNotIs<ObjectReference>(propertyType, "propertType");
			}

			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
		{
			if (value == null||string.IsNullOrEmpty(Field.LookupList))
				return null;

			if (!Field.AllowMultipleValues)
			{
				var fieldValue = new SPFieldLookupValue(value.ToString());

				return new ObjectReference(new Guid(Field.LookupList), fieldValue.LookupId, fieldValue.LookupValue);
			}

			var fieldValues = new SPFieldLookupValueCollection(value.ToString());

			return fieldValues.Select(fieldValue => new ObjectReference(new Guid(Field.LookupList), fieldValue.LookupId, fieldValue.LookupValue)).ToList();
		}

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
	}
}
