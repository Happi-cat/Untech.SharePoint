using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.Custom
{
	/// <summary>
	/// Represents field converter that can convert string to <see cref="Enum"/> and vice versa.
	/// </summary>
	public sealed class EnumFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }

		/// <summary>
		/// Initialzes current instance with the specified <see cref="MetaField"/>
		/// </summary>
		/// <param name="field"></param>
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (!field.MemberType.IsEnum)
				throw new ArgumentException("This converter can be used only with Enum property types");

			if (!Enum.IsDefined(field.MemberType, 0))
				throw new ArgumentException(string.Format("Enum {0} should have default value (i.e. 0)", field.MemberType));

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

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP field value.
		/// </summary>
		/// <param name="value">Member value to convert.</param>
		/// <returns>SP field value.</returns>
		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var enumName = Enum.GetName(Field.MemberType, value);

			var enumMemberAttribute = Field.MemberType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

			return enumMemberAttribute != null ? enumMemberAttribute.Value : enumName;
		}

		string IFieldConverter.ToCamlValue(object value)
		{
			return (string)ToSpValue(value);
		}
	}
}