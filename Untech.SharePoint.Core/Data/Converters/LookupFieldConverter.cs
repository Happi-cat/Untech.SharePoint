using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Lookup")]
	[SPFieldConverter("LookupMulti")]
	internal class LookupFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Field = field;
			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
		{
			var lookupfield = Field as SPFieldLookup;
			if(lookupfield == null)
			{
				throw new ArgumentException();
			}

			if (value == null||string.IsNullOrEmpty(lookupfield.LookupList))
				return null;

			if (!lookupfield.AllowMultipleValues)
			{
				var fieldValue = new SPFieldLookupValue(value.ToString());

				return new ObjectReference(new Guid(lookupfield.LookupList), fieldValue.LookupId, fieldValue.LookupValue);
			}

			var fieldValues = new SPFieldLookupValueCollection(value.ToString());

			return fieldValues.Select(fieldValue => new ObjectReference(new Guid(lookupfield.LookupList), fieldValue.LookupId, fieldValue.LookupValue)).ToList();
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;
			
			var lookupfield = Field as SPFieldLookup;
			if (lookupfield == null || (!(value is ObjectReference) && !(value is IList<ObjectReference>)))
			{
				throw new ArgumentException();
			}

			if (!lookupfield.AllowMultipleValues)
			{
				var reference = value as ObjectReference;

				return new SPFieldLookupValue(reference.Id, reference.Value);
			}

			var references = value as IList<ObjectReference>;

			var fieldValues = new SPFieldLookupValueCollection();
			fieldValues.AddRange(references.Select(referenceInfo => new SPFieldLookupValue(referenceInfo.Id, referenceInfo.Value)));

			return fieldValues;
		}
	}
}
