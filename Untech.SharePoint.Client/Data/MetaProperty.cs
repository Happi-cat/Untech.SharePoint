using System;
using System.ComponentModel;
using System.Reflection;
using Untech.SharePoint.Client.Data.FieldConverters;

namespace Untech.SharePoint.Client.Data
{
	internal class MetaProperty
	{
		public MetaProperty(PropertyInfo propertyInfo)
		{
			MemberName = propertyInfo.Name;
			MemberType = propertyInfo.PropertyType;

			UpdateSpFieldInfo(propertyInfo);
			UpdateDefaultValue(propertyInfo);
			RegisterCustomConverter();
		}

		public MetaProperty(FieldInfo fieldInfo)
		{
			MemberName = fieldInfo.Name;
			MemberType = fieldInfo.FieldType;

			UpdateSpFieldInfo(fieldInfo);
			UpdateDefaultValue(fieldInfo);
			RegisterCustomConverter();
		}

		public string MemberName { get; set; }

		public Type MemberType { get; set; }

		public string SpFieldInternalName { get; set; }

		public Type CustomConverterType { get; set; }

		public object DefaultValue { get; set; }


		private void UpdateSpFieldInfo(MemberInfo memberInfo)
		{
			var attribute = memberInfo.GetCustomAttribute<SpFieldAttribute>();
			if (attribute == null)
			{
				throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", memberInfo.Name), "memberInfo");
			}

			SpFieldInternalName = attribute.InternalName ?? MemberName;
			CustomConverterType = attribute.CustomConverterType;
		}

		private void UpdateDefaultValue(MemberInfo memberInfo)
		{
			var attribute = memberInfo.GetCustomAttribute<DefaultValueAttribute>();
			if (attribute == null)
			{
				return;
			}

			DefaultValue = attribute.Value;
		}

		private void RegisterCustomConverter()
		{
			if (CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(CustomConverterType);
			}
		}
	}
}