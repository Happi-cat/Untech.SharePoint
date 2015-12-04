using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
{
	[SpFieldConverter("UserMulti")]
	[UsedImplicitly]
	internal class UserMultiFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }
		private bool IsArray { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (field.MemberType != typeof(UserInfo[]) && !field.MemberType.IsAssignableFrom(typeof(List<UserInfo>)))
			{
				throw new ArgumentException(
					"Only UserInfo[] or any class assignable from List<UserInfo> can be used as a member type.");
			}
			Field = field;
			IsArray = field.MemberType.IsArray;
		}


		public object FromSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var fieldValues = (IEnumerable<FieldUserValue>)value;
			var users = fieldValues.Select(ConvertToUserInfo);

			return IsArray ? (object)users.ToArray() : users.ToList();
		}

		public object ToSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var userInfos = (IEnumerable<UserInfo>)value;

			return userInfos.Select(n => new FieldUserValue{ LookupId = n.Id }).ToList();
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return "";

			var userInfos = (IEnumerable<UserInfo>)value;

			return userInfos.Select(n => string.Format("{0};#{1}", n.Id, n.Login)).JoinToString(";#");
		}

		private UserInfo ConvertToUserInfo(FieldUserValue user)
		{
			return new UserInfo
			{
				Id = user.LookupId,
				Login = user.LookupValue,
				Email = user.Email
			};
		}
	}
}