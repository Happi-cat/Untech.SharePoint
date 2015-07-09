using System;
using System.ComponentModel;
using System.Reflection;
using Untech.SharePoint.Core.Data.Converters;

namespace Untech.SharePoint.Core.Data
{
	internal sealed class MetaProperty
	{
		public string MemberName { get; set; }

		public Type MemberType { get; set; }

		public string SpFieldInternalName { get; set; }

		public Type CustomConverterType { get; set; }

		public object DefaultValue { get; set; }

		public static MetaProperty Create(PropertyInfo propertyInfo)
		{
			var info = new MetaProperty
			{
				MemberName = propertyInfo.Name,
				MemberType = propertyInfo.PropertyType,
			};

			UpdateSPFieldInfo(propertyInfo, info);
			UpdateDefaultValue(propertyInfo, info);
			RegisterCustomConverter(info);

			return info;
		}

		public static MetaProperty Create(FieldInfo fieldInfo)
		{
			var info = new MetaProperty
			{
				MemberName = fieldInfo.Name,
				MemberType = fieldInfo.FieldType,
			};

			UpdateSPFieldInfo(fieldInfo, info);
			UpdateDefaultValue(fieldInfo, info);
			RegisterCustomConverter(info);

			return info;
		}

		private static void UpdateSPFieldInfo(MemberInfo memberInfo, MetaProperty info)
		{
			var fieldAttribute = memberInfo.GetCustomAttribute<SpFieldAttribute>();
			if (fieldAttribute == null)
			{
				throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", memberInfo.Name), "memberInfo");
			}

			info.SpFieldInternalName = fieldAttribute.InternalName ?? info.MemberName;
			info.CustomConverterType = fieldAttribute.CustomConverterType;
		}

		private static void UpdateDefaultValue(MemberInfo memberInfo, MetaProperty info)
		{
			var defaultAttribute = memberInfo.GetCustomAttribute<DefaultValueAttribute>();
			if (defaultAttribute == null) return;

			info.DefaultValue = defaultAttribute.Value;
		}

		private static void RegisterCustomConverter(MetaProperty info)
		{
			if (info.CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(info.CustomConverterType);
			}
		}
	}
}