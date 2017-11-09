using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Converters;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.Models;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
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
			Guard.CheckNotNull(nameof(field), field);

			Field = field;

			if (field.AllowMultipleValues)
			{
				if (field.MemberType != typeof(ObjectReference[])
					&& !field.MemberType.IsAssignableFrom(typeof(List<ObjectReference>)))
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
				return ConvertToObjRef((FieldLookupValue)value);
			}

			var fieldValues = (IEnumerable<FieldLookupValue>)value;
			var lookupValues = fieldValues.Select(ConvertToObjRef).ToList();

			if (lookupValues.Count == 0)
			{
				return null;
			}
			return IsArray ? (object)lookupValues.ToArray() : lookupValues;
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			if (!IsMulti)
			{
				var lookupValue = (ObjectReference)value;

				return new FieldLookupValue { LookupId = lookupValue.Id };
			}

			var lookupValues = ((IEnumerable<ObjectReference>)value).Distinct().ToList();

			if (lookupValues.Count == 0)
			{
				return null;
			}
			return lookupValues
				.Select(n => new FieldLookupValue { LookupId = n.Id })
				.ToList();
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return null;

			if (value is ObjectReference singleValue)
			{
				return singleValue.Id.ToString();
			}

			var multiValue = (IEnumerable<ObjectReference>)value;
			return multiValue
				.Distinct()
				.Select(n => string.Format("{0};#{1}", n.Id, n.Value))
				.JoinToString(";#");
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
