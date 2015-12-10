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
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("User")]
	[SpFieldConverter("UserMulti")]
	[UsedImplicitly]
	internal class UserFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }
		private SPWeb Web { get; set; }
		private bool IsMulti { get; set; }
		private bool IsArray { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;

			if (field.AllowMultipleValues)
			{
				if (field.MemberType != typeof(UserInfo[]) &&
					!field.MemberType.IsAssignableFrom(typeof(List<UserInfo>)))
				{
					throw new ArgumentException(
						"Only UserInfo[] or any class assignable from List<UserInfo> can be used as a member type.");
				}

				IsMulti = true;
				IsArray = field.MemberType.IsArray;
			}
			else
			{
				if (field.MemberType != typeof(UserInfo))
				{
					throw new ArgumentException(
						"Only UserInfo can be used as a member type.");
				}
			}

			Web = field.GetSpWeb();
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			if (!IsMulti)
			{
				return ConvertToUserInfo(new SPFieldUserValue(Web, value.ToString()));
			}

			var fieldValues = new SPFieldUserValueCollection(Web, value.ToString());
			var userValues = fieldValues.Select(ConvertToUserInfo);

			return IsArray ? (object)userValues.ToArray() : userValues.ToList();
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			if (!IsMulti)
			{
				var userValue = (UserInfo)value;

				var fieldValue = new SPFieldUserValue(Web, userValue.Id, userValue.Login);
				return fieldValue.ToString();
			}

			var userValues = (IEnumerable<UserInfo>)value;

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(userValues.Select(n => new SPFieldUserValue(Web, n.Id, n.Login)));

			return fieldValues;
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return null;

			var singleValue = value as UserInfo;
			if (singleValue != null)
			{
				return singleValue.Id.ToString();
			}

			var multiValue = (IEnumerable<UserInfo>)value;
			return multiValue
				.Select(n => string.Format("{0};#{1}", n.Id, n.Login))
				.JoinToString(";#");
		}

		private UserInfo ConvertToUserInfo(SPFieldUserValue userValue)
		{
			return new UserInfo
			{
				Id = userValue.LookupId,
				Login = userValue.LookupValue,
				Email = userValue.User.Email
			};
		}
	}
}
