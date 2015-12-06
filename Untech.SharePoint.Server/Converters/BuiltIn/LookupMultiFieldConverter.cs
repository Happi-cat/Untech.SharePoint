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
	[SpFieldConverter("LookupMulti")]
	[UsedImplicitly]
	internal class LookupMultiFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }
		private bool IsArray { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (field.MemberType != typeof(ObjectReference[]) && !field.MemberType.IsAssignableFrom(typeof(List<ObjectReference>)))
			{
				throw new ArgumentException(
					"Only ObjectReference[] or any class assignable from List<ObjectReference> can be used as a member type.");
			}
			Field = field;
			IsArray = field.MemberType.IsArray;
		}

		public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			var fieldValues = new SPFieldLookupValueCollection(value.ToString());

			var lookups = fieldValues.Select(ConvertToObjRef);
			return IsArray? (object)lookups.ToArray() : lookups.ToList();
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;
			
			var references = (IEnumerable<ObjectReference>)value;

			var fieldValues = new SPFieldLookupValueCollection();
			fieldValues.AddRange(references.Select(n => new SPFieldLookupValue(n.Id, n.Value)));

			return fieldValues;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
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