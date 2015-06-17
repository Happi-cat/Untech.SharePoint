using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	public class EnumFieldConverter : IFieldConverter
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
			if (!PropertyType.IsEnum) throw new ArgumentException("property should be Enum");

			if (value == null)
			{
				if (Enum.IsDefined(PropertyType, 0))
				{
					return 0;
				}
				throw new InvalidEnumArgumentException("value", 0, PropertyType);
			}

			var enumString = value.ToString();

			foreach (var enumName in Enum.GetNames(PropertyType))
			{
				var enumMemberAttribute = PropertyType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

				if (enumMemberAttribute != null && string.Compare(enumMemberAttribute.Value, enumString, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return Enum.Parse(PropertyType, enumName);
				}

				if (string.Compare(enumName, enumString, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return Enum.Parse(PropertyType, enumName);
				}
			}

			throw new InvalidEnumArgumentException("value");
		}

		public object ToSpValue(object value)
		{
			if (!PropertyType.IsEnum) throw new ArgumentException("property should be Enum");

			if (value == null)
				return null;

			var enumName = Enum.GetName(PropertyType, value);

			var enumMemberAttribute = PropertyType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

			return enumMemberAttribute != null ? enumMemberAttribute.Value : enumName;
		}
	}
}