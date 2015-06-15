using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("User")]
	internal class UserFieldConverter: IFieldConverter
	{
	    public object FromSpValue(object value, SPField field, Type propertyType)
		{
			if (value == null)
				return null;

			var userField = field as SPFieldUser;
			if (userField == null)
			{
				throw new ArgumentException();
			}

			if (!userField.AllowMultipleValues)
			{
				var fieldValue = new SPFieldUserValue(userField.ParentList.ParentWeb, value.ToString());

				return new UserInfo(fieldValue.User);
			}

			var fieldValues = new SPFieldUserValueCollection(userField.ParentList.ParentWeb, value.ToString());

			return fieldValues.Select(fieldValue => fieldValue.User).Select(user => new UserInfo(user)).ToList();
		}

        public object ToSpValue(object value, SPField field, Type propertyType)
		{
			if (value == null)
				return null;

			var userField = field as SPFieldUser;
			if (userField == null || (!(value is UserInfo) && !(value is IList<UserInfo>)))
			{
				throw new ArgumentException();
			}

			if (!userField.AllowMultipleValues)
			{
				var userInfo = value as UserInfo;

				return new SPFieldUserValue(field.ParentList.ParentWeb, userInfo.Id, userInfo.Login);
			}

			var userInfos = value as IList<UserInfo>;

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(userInfos.Select(userInfo => new SPFieldUserValue(field.ParentList.ParentWeb, userInfo.Id, userInfo.Login)));

			return fieldValues;
		}

	}
}
