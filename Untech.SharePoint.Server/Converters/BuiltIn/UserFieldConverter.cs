using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("User")]
	[UsedImplicitly]
	internal class UserFieldConverter : IFieldConverter
	{
		private SPWeb Web { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (field.MemberType != typeof (UserInfo))
			{
				throw new ArgumentException("Only UserInfo can be used as a member type.");
			}
			Web = field.GetSpWeb();
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			var fieldValue = new SPFieldUserValue(Web, value.ToString());

			return ConvertToUserInfo(fieldValue);
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			var userValue = (UserInfo) value;

			var fieldValue = new SPFieldUserValue(Web, userValue.Id, userValue.Login);
			return fieldValue.ToString();
		}

		public string ToCamlValue(object value)
		{
			return (string) ToSpValue(value) ?? "";
		}


		private UserInfo ConvertToUserInfo(SPFieldUserValue user)
		{
			return new UserInfo
			{
				Id = user.LookupId,
				Login = user.LookupValue,
				Email = user.User.Email
			};
		}
	}
}
