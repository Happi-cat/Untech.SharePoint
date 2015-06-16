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
			var fieldAttribute = fieldInfo.GetCustomAttribute<SPFieldAttribute>();
			var defaultAttribute = fieldInfo.GetCustomAttribute<DefaultValueAttribute>();

			var info = new PropertyMappingInfo
			{
				PropertyOrFieldName = fieldInfo.Name,
				PropertyOrFieldType = fieldInfo.FieldType,
				SPFieldInternalName = fieldAttribute.InternalName,
				CustomConverterType = fieldAttribute.CustomConverterType
			};

			if (defaultAttribute != null)
			{
				info.DefaultValue = defaultAttribute.Value;
			}

			if (info.CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(info.CustomConverterType);
			}

			_mappings.Add(info);
		}

		private void AddMappingInfo(PropertyInfo propertyInfo)
		{
			var fieldAttribute = propertyInfo.GetCustomAttribute<SPFieldAttribute>();
			var defaultAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();

			var info = new PropertyMappingInfo
			{
				PropertyOrFieldName = propertyInfo.Name,
				PropertyOrFieldType = propertyInfo.PropertyType,
				SPFieldInternalName = fieldAttribute.InternalName,
				CustomConverterType = fieldAttribute.CustomConverterType
			};

			if (defaultAttribute != null)
			{
				info.DefaultValue = defaultAttribute.Value;
			}

			if (info.CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(info.CustomConverterType);
			}

			_mappings.Add(info);
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