using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Extensions;
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
		private bool IsMulti { get; set; }
		private bool IsArray { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;

			if (field.AllowMultipleValues)
			{
				if (field.MemberType != typeof(ObjectReference[]) &&
					!field.MemberType.IsAssignableFrom(typeof(List<ObjectReference>)))
				{
					throw new ArgumentException(
						"Only ObjectReference[] or any class assignable from List<ObjectReference> can be used as a member type.");
				}

				IsMulti = true;
				IsArray = field.MemberType.IsArray;
			}
			else
			{
				if (field.MemberType != typeof(ObjectReference))
				{
					throw new ArgumentException(
						"Only ObjectReference can be used as a member type.");
				}
			}
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			if (!IsMulti)
			{
				return ConvertToObjRef(new SPFieldLookupValue(value.ToString()));
			}

			var fieldValues = new SPFieldLookupValueCollection(value.ToString());
			var lookupValues = fieldValues.Select(ConvertToObjRef).ToList();
			if (!lookupValues.Any())
			{
				return null;
			}

			return IsArray ? (object) lookupValues.ToArray() : lookupValues;
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			if (!IsMulti)
			{
				var lookupValue = (ObjectReference)value;

				var fieldValue = new SPFieldLookupValue(lookupValue.Id, lookupValue.Value);
				return fieldValue.ToString();
			}

			var lookupValues = ((IEnumerable<ObjectReference>)value).ToList();
			if (!lookupValues.Any())
			{
				return null;
			}

			var fieldValues = new SPFieldLookupValueCollection();
			fieldValues.AddRange(lookupValues.Select(n => new SPFieldLookupValue(n.Id, n.Value)));

			return fieldValues;
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return null;

			var singleValue = value as ObjectReference;
			if (singleValue != null)
			{
				return singleValue.Id.ToString();
			}

			var multiValue = (IEnumerable<ObjectReference>)value;
			return multiValue
				.Select(n => string.Format("{0};#{1}", n.Id, n.Value))
				.JoinToString(";#");
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
