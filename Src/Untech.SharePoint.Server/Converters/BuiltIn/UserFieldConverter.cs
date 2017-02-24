using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("User")]
	[SpFieldConverter("UserMulti")]
	[UsedImplicitly]
	internal class UserFieldConverter : IFieldConverter
	{
		private SPWeb _web;
		private bool _isMulti;
		private bool _isArray;

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull(nameof(field), field);

			if (field.AllowMultipleValues)
			{
				if (field.MemberType != typeof(UserInfo[])
					&& !field.MemberType.IsAssignableFrom(typeof(List<UserInfo>)))
				{
					throw new ArgumentException(
						"Only UserInfo[] or any class assignable from List<UserInfo> can be used as a member type.");
				}

				_isMulti = true;
				_isArray = field.MemberType.IsArray;
			}
			else
			{
				if (field.MemberType != typeof(UserInfo))
				{
					throw new ArgumentException(
						"Only UserInfo can be used as a member type.");
				}
			}

			_web = field.GetSpWeb();
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			if (!_isMulti)
			{
				return ConvertToUserInfo(new SPFieldUserValue(_web, value.ToString()));
			}

			var fieldValues = new SPFieldUserValueCollection(_web, value.ToString());
			var userValues = fieldValues.Select(ConvertToUserInfo).ToList();

			if (!userValues.Any())
			{
				return null;
			}
			return _isArray ? (object)userValues.ToArray() : userValues;
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			if (!_isMulti)
			{
				var userValue = (UserInfo)value;

				var fieldValue = new SPFieldUserValue(_web, userValue.Id, userValue.Login);
				return fieldValue.ToString();
			}

			var userValues = ((IEnumerable<UserInfo>)value).Distinct().ToList();
			if (!userValues.Any())
			{
				return null;
			}

			var fieldValues = new SPFieldUserValueCollection();
			fieldValues.AddRange(userValues.Select(n => new SPFieldUserValue(_web, n.Id, n.Login)));

			return fieldValues;
		}

		public string ToCamlValue(object value)
		{
			if (value == null) return null;

			var singleValue = value as UserInfo;
			if (singleValue != null)
			{
				return singleValue.Id.ToString();
			}

			var multiValue = (IEnumerable<UserInfo>)value;
			return multiValue
				.Distinct()
				.Select(n => string.Format("{0};#{1}", n.Id, n.Login))
				.JoinToString(";#");
		}

		private UserInfo ConvertToUserInfo(SPFieldUserValue userValue)
		{
			return new UserInfo
			{
				Id = userValue.LookupId,
				Login = userValue.LookupValue,
				//Email = userValue.User.Email
			};
		}
	}
}
