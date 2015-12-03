using System;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
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

			return ConvertToObjRef((FieldLookupValue) value);
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			var lookupValue = (ObjectReference)value;

			return new FieldLookupValue{ LookupId = lookupValue.Id };
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return null;

			var lookupValue = (ObjectReference)value;

			return string.Format("{0};#{1}", lookupValue.Id, lookupValue.Value);
		}

		private ObjectReference ConvertToObjRef(FieldLookupValue lookupValue)
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
