using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.Custom
{
	public class EnumFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.NotNull(field, "field");
			Guard.NotNull(propertyType, "propertyType");

			Guard.TypeIs<string>(field.FieldValueType, "field.FieldValueType");

			if (!propertyType.IsEnum)
				throw new ArgumentException("This converter can be used only with Enum property types");

			if (!Enum.IsDefined(propertyType, 0))
				throw new ArgumentException(string.Format("Enum {0} should have default value (i.e. 0)", propertyType));

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			if (value == null)
			{
				return 0;
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
			if (value == null)
				return null;

			var enumName = Enum.GetName(PropertyType, value);

			var enumMemberAttribute = PropertyType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

			return enumMemberAttribute != null ? enumMemberAttribute.Value : enumName;
		}
	}
}