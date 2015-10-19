using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.Custom
{
	public class EnumFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (!field.MemberType.IsEnum)
				throw new ArgumentException("This converter can be used only with Enum property types");

			if (!Enum.IsDefined(field.MemberType, 0))
				throw new ArgumentException(string.Format("Enum {0} should have default value (i.e. 0)", field.MemberType));

			Field = field;
		}

		public object FromSpValue(object value)
		{
			if (value == null)
			{
				return 0;
			}

			var enumString = value.ToString();

			foreach (var enumName in Enum.GetNames(Field.MemberType))
			{
				var enumMemberAttribute = Field.MemberType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

				if (enumMemberAttribute != null && string.Compare(enumMemberAttribute.Value, enumString, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return Enum.Parse(Field.MemberType, enumName);
				}

				if (string.Compare(enumName, enumString, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return Enum.Parse(Field.MemberType, enumName);
				}
			}

			throw new InvalidEnumArgumentException("value");
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var enumName = Enum.GetName(Field.MemberType, value);

			var enumMemberAttribute = Field.MemberType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

			return enumMemberAttribute != null ? enumMemberAttribute.Value : enumName;
		}

		public string ToCamlValue(object value)
		{
			return (string)ToSpValue(value);
		}
	}
}