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
	[SpFieldConverter("User")]
	[UsedImplicitly]
	internal class UserFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }
		private Type PropertyType { get; set; }

		/// <summary>
		/// Initialzes current instance with the specified <see cref="MetaField"/>
		/// </summary>
		/// <param name="field"></param>
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		/// <summary>
		/// Converts SP field value to <see cref="MetaField.MemberType"/>.
		/// </summary>
		/// <param name="value">SP value to convert.</param>
		/// <returns>Member value.</returns>
		public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			if (!Field.AllowMultipleValues)
			{
				var fieldValue = new SPFieldUserValue(Field.GetSpWeb(), value.ToString());

				return UserToUserInfo(fieldValue.User);
			}

			var fieldValues = new SPFieldUserValueCollection(Field.GetSpWeb(), value.ToString());
			var users = fieldValues.Select(fieldValue => fieldValue.User).Select(UserToUserInfo);

			return PropertyType == typeof (UserInfo[]) ? (object) users.ToArray() : users.ToList();
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP field value.
		/// </summary>
		/// <param name="value">Member value to convert.</param>
		/// <returns>SP field value.</returns>
		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			if (!Field.AllowMultipleValues)
			{
				return UserInfoToFieldValue((UserInfo) value);
			}

			var userInfos = (IEnumerable<UserInfo>) value;

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(userInfos.Select(UserInfoToFieldValue));

			return fieldValues;
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP Caml value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Caml value.</returns>
		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}

		private SPFieldUserValue UserInfoToFieldValue(UserInfo userInfo)
		{
			return new SPFieldUserValue(Field.GetSpWeb(), userInfo.Id, userInfo.Login);
		}

		private static UserInfo UserToUserInfo(SPUser user)
		{
			return new UserInfo
			{
				Id = user.ID
			};
		}
	}
}
