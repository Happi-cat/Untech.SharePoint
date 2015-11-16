using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
{
	[SpFieldConverter("User")]
	internal class UserFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }
		public Type PropertyType { get; set; }

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
				return FieldValueToUserInfo((FieldUserValue) value);
			}

			var fieldValues = (IEnumerable<FieldUserValue>) value;
			var users = fieldValues.Select(FieldValueToUserInfo);

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
				var userInfo = (UserInfo) value;

				return UserInfoToFieldValue(userInfo);
			} 

			var userInfos = (IEnumerable<UserInfo>) value;

			var fieldValues = new List<FieldUserValue>();
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
			throw new NotImplementedException();
		}

		private static UserInfo FieldValueToUserInfo(FieldUserValue fieldValue)
		{
			return new UserInfo
			{
				Id = fieldValue.LookupId,
				Email = fieldValue.Email
			};
		}

		private static FieldUserValue UserInfoToFieldValue(UserInfo userInfo)
		{
			return new FieldUserValue
			{
				LookupId = userInfo.Id
			};
		}
	}
}
