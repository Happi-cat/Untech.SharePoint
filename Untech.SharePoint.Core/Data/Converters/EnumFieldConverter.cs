using System;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	public class EnumFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			if (!propertyType.IsEnum) throw new ArgumentException("property should be Enum");

			if (value == null)
			{
				return propertyType.IsNullableType() ? null : (object)0;
			}

			var enumString = value.ToString();

			foreach (var enumName in Enum.GetNames(propertyType))
			{
				var enumMemberAttribute = propertyType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

				if (enumMemberAttribute != null && string.Compare(enumMemberAttribute.Value, enumString, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return Enum.Parse(propertyType, enumName);
				}

				if (string.Compare(enumName, enumString, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return Enum.Parse(propertyType, enumName);
				}
			}

			return 0;
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			if (!propertyType.IsEnum) throw new ArgumentException("property should be Enum");

			if (value == null)
				return null;

			var enumName = Enum.GetName(propertyType, value);

			var enumMemberAttribute = propertyType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

			return enumMemberAttribute != null ? enumMemberAttribute.Value : enumName;
		}
	}
}