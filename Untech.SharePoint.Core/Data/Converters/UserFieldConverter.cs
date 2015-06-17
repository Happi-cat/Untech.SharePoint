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
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Field = field;
			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			var userField = Field as SPFieldUser;
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

        public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var userField = Field as SPFieldUser;
			if (userField == null || (!(value is UserInfo) && !(value is IList<UserInfo>)))
			{
				throw new ArgumentException();
			}

			if (!userField.AllowMultipleValues)
			{
				var userInfo = value as UserInfo;

				return new SPFieldUserValue(Field.ParentList.ParentWeb, userInfo.Id, userInfo.Login);
			}

			var userInfos = value as IList<UserInfo>;

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(userInfos.Select(userInfo => new SPFieldUserValue(Field.ParentList.ParentWeb, userInfo.Id, userInfo.Login)));

			return fieldValues;
		}

	}
}
