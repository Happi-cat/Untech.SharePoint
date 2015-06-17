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
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (!(field is SPFieldUser))
				throw new ArgumentException("SPFieldUser only supported");

			if (!propertyType.IsAssignableFrom(typeof(List<UserInfo>)) && propertyType != typeof(UserInfo[]) && propertyType != typeof(UserInfo))
				throw new ArgumentException("This converter can be used only with string[] or with types assignable from List<string>");


			Field = field;
			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			var userField = Field as SPFieldUser;

			if (!userField.AllowMultipleValues)
			{
				var fieldValue = new SPFieldUserValue(userField.ParentList.ParentWeb, value.ToString());

				return new UserInfo(fieldValue.User);
			}

			var fieldValues = new SPFieldUserValueCollection(userField.ParentList.ParentWeb, value.ToString());

			if (PropertyType == typeof(UserInfo[]))
				return fieldValues.Select(fieldValue => fieldValue.User).Select(user => new UserInfo(user)).ToArray();
			
			return fieldValues.Select(fieldValue => fieldValue.User).Select(user => new UserInfo(user)).ToList();
		}

        public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var userField = Field as SPFieldUser;
		
			if (!userField.AllowMultipleValues)
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
