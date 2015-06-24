using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Core.Data.Converters;

namespace Untech.SharePoint.Core.Data
{
	internal class PropertyMappings : IEnumerable<PropertyMappingInfo>
	{
		private readonly List<PropertyMappingInfo> _mappings = new List<PropertyMappingInfo>();

		public void Initialize(Type objectType)
		{
			const BindingFlags bindingflags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
			var attributeType = typeof (SPFieldAttribute);

			var properties = objectType.GetProperties(bindingflags)
				.Where(n => n.IsDefined(attributeType))
				.Where(n => n.CanRead && n.CanWrite)
				.ToList();

			var fields = objectType.GetFields(bindingflags)
				.Where(n => n.IsDefined(attributeType))
				.ToList();

			properties.ForEach(AddMappingInfo);
			fields.ForEach(AddMappingInfo);
		}

		private void AddMappingInfo(FieldInfo fieldInfo)
		{
			var info = new PropertyMappingInfo
			{
				PropertyOrFieldName = fieldInfo.Name,
				PropertyOrFieldType = fieldInfo.FieldType,
			};

			UpdateSPFieldInfo(fieldInfo, info);
			UpdateDefaultValue(fieldInfo, info);
			RegisterCustomConverter(info);

			_mappings.Add(info);
		}

		private void AddMappingInfo(PropertyInfo propertyInfo)
		{
			var info = new PropertyMappingInfo
			{
				PropertyOrFieldName = propertyInfo.Name,
				PropertyOrFieldType = propertyInfo.PropertyType,
			};

			UpdateSPFieldInfo(propertyInfo, info);
			UpdateDefaultValue(propertyInfo, info);
			RegisterCustomConverter(info);
			
			_mappings.Add(info);
		}

		private void UpdateSPFieldInfo(MemberInfo memberInfo, PropertyMappingInfo mappingInfo)
		{
			var fieldAttribute = memberInfo.GetCustomAttribute<SPFieldAttribute>();
			if (fieldAttribute == null) return;

			mappingInfo.SPFieldInternalName = fieldAttribute.InternalName;
			mappingInfo.CustomConverterType = fieldAttribute.CustomConverterType;
		}

		private void UpdateDefaultValue(MemberInfo memberInfo, PropertyMappingInfo mappingInfo)
		{
			var defaultAttribute = memberInfo.GetCustomAttribute<DefaultValueAttribute>();
			if (defaultAttribute == null) return;

			mappingInfo.DefaultValue = defaultAttribute.Value;
		}

		private void RegisterCustomConverter(PropertyMappingInfo mappingInfo)
		{
			if (mappingInfo.CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(mappingInfo.CustomConverterType);
			}

		}

		public IEnumerator<PropertyMappingInfo> GetEnumerator()
		{
			return  _mappings.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}