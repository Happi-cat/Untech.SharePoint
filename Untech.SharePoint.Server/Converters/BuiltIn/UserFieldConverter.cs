using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
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
				var fieldValue = new SPFieldUserValue(Field.GetAdditionalProperty<SPWeb>("SPWeb"), value.ToString());

				return new UserInfo
				{
					Id = fieldValue.User.ID
				};
			}

			var fieldValues = new SPFieldUserValueCollection(Field.GetAdditionalProperty<SPWeb>("SPWeb"), value.ToString());
			var users = fieldValues.Select(fieldValue => fieldValue.User).Select(user => new UserInfo
			{
				Id = user.ID
			});

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

				return new SPFieldUserValue(Field.GetAdditionalProperty<SPWeb>("SPWeb"), userInfo.Id, userInfo.Login);
			}

			var userInfos = (IEnumerable<UserInfo>) value;

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(
				userInfos.Select(
					userInfo => new SPFieldUserValue(Field.GetAdditionalProperty<SPWeb>("SPWeb"), userInfo.Id, userInfo.Login)));

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
	}
}
