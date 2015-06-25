using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("User")]
	internal class UserFieldConverter: IFieldConverter
	{
		public SPFieldUser Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.NotNull(field, "field");
			Guard.NotNull(propertyType, "propertyType");

			Field = field as SPFieldUser;
			if (Field == null)
				throw new ArgumentException("SPFieldUser only supported");

			if (Field.AllowMultipleValues)
			{
				Guard.ArrayOrAssignableFromList<UserInfo>(propertyType, "propertType");
			}
			else
			{
				Guard.TypeIs<UserInfo>(propertyType, "propertyType");
			}

			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			if (!Field.AllowMultipleValues)
			{
				var fieldValue = new SPFieldUserValue(Field.ParentList.ParentWeb, value.ToString());

				return new UserInfo(fieldValue.User);
			}

			var fieldValues = new SPFieldUserValueCollection(Field.ParentList.ParentWeb, value.ToString());
		    var users = fieldValues.Select(fieldValue => fieldValue.User).Select(user => new UserInfo(user));

		    return PropertyType == typeof (UserInfo[]) ? (object) users.ToArray() : users.ToList();
		}

        public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			if (!Field.AllowMultipleValues)
			{
				var userInfo = value as UserInfo;

				return new SPFieldUserValue(Field.ParentList.ParentWeb, userInfo.Id, userInfo.Login);
			}

			var userInfos = (IEnumerable<UserInfo>)value;

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(userInfos.Select(userInfo => new SPFieldUserValue(Field.ParentList.ParentWeb, userInfo.Id, userInfo.Login)));

			return fieldValues;
		}

	}
}
