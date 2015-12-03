using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("Lookup")]
	[UsedImplicitly]
	internal class LookupFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (field.MemberType != typeof(ObjectReference))
			{
				throw new ArgumentException("Only ObjectReference can be used as a member type.");
			}
			Field = field;
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			var fieldValue = new SPFieldLookupValue(value.ToString());

			return ConvertToObjRef(fieldValue);
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			var lookupValue = (ObjectReference)value;

			var fieldValue = new SPFieldLookupValue(lookupValue.Id, lookupValue.Value);
			return fieldValue.ToString();
		}

		public string ToCamlValue(object value)
		{
			return (string)ToSpValue(value) ?? "";
		}

		private ObjectReference ConvertToObjRef(SPFieldLookupValue lookupValue)
		{
			return new ObjectReference
			{
				Id = lookupValue.LookupId,
				ListId = new Guid(Field.LookupList),
				Value = lookupValue.LookupValue
			};
		}
	}
}
