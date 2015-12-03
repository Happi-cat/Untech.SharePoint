using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("UserMulti")]
	[UsedImplicitly]
	internal class UserMultiFieldConverter : IFieldConverter
	{
		private Type MemberType { get; set; }
		private SPWeb Web { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (field.MemberType != typeof (UserInfo[]) && !field.MemberType.IsAssignableFrom(typeof (List<UserInfo>)))
			{
				throw new ArgumentException(
					"Only UserInfo[] or any class assignable from List<UserInfo> can be used as a member type.");
			}
			Web = field.GetSpWeb();
			MemberType = field.MemberType;
		}


		public object FromSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var fieldValues = new SPFieldUserValueCollection(Web, value.ToString());
			var users = fieldValues.Select(ConvertToUserInfo);

			return MemberType == typeof (UserInfo[]) ? (object) users.ToArray() : users.ToList();
		}

		public object ToSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var userInfos = (IEnumerable<UserInfo>) value;

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(userInfos.Select(n => new SPFieldUserValue(Web, n.Id, n.Login)));

			return fieldValues;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}

		private static UserInfo ConvertToUserInfo(SPFieldUserValue user)
		{
			return new UserInfo
			{
				Id = user.LookupId,
				Login = user.LookupValue
			};
		}
	}
}