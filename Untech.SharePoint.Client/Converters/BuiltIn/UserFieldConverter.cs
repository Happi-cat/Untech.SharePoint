using System;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
{
	[SpFieldConverter("User")]
	[UsedImplicitly]
	internal class UserFieldConverter : IFieldConverter
	{
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (field.MemberType != typeof(UserInfo))
			{
				throw new ArgumentException("Only UserInfo can be used as a member type.");
			}
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			var fieldValue = (FieldUserValue) value;

			return ConvertToUserInfo(fieldValue);
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			var userValue = (UserInfo)value;

			return new FieldUserValue{ LookupId = userValue.Id };
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return null;

			var userValue = (UserInfo)value;

			return string.Format("{0};#{1}", userValue.Id, userValue.Login);
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
