using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Core.Data.Converters;

namespace Untech.SharePoint.Core.Data
{
	internal class DataModelPropertyInfos : IReadOnlyDictionary<string, DataModelPropertyInfo>
	{
		private readonly Dictionary<string, DataModelPropertyInfo> _mappings = new Dictionary<string, DataModelPropertyInfo>();

		public void Initialize(Type objectType)
		{
			const BindingFlags bindingflags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
			var attributeType = typeof (SpFieldAttribute);

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

		public DataModelPropertyInfo this[string propertyOrFieldName]
		{
			get { return _mappings[propertyOrFieldName]; }
		}

		public bool ContainsKey(string propertyOrFieldName)
		{
			return _mappings.ContainsKey(propertyOrFieldName);
		}

		public IEnumerable<string> Keys
		{
			get { return _mappings.Keys; }
		}

		public bool TryGetValue(string propertyOrFieldName, out DataModelPropertyInfo value)
		{
			return _mappings.TryGetValue(propertyOrFieldName, out value);
		}

		public IEnumerable<DataModelPropertyInfo> Values
		{
			get { return _mappings.Values; }
		}

		public int Count
		{
			get { return _mappings.Count; }
		}


		public IEnumerator GetEnumerator()
		{
			return _mappings.GetEnumerator();
		}

		IEnumerator<KeyValuePair<string, DataModelPropertyInfo>> IEnumerable<KeyValuePair<string, DataModelPropertyInfo>>.GetEnumerator()
		{
			return _mappings.GetEnumerator();
		}

		#region [Private Methods]

		private void AddMappingInfo(FieldInfo fieldInfo)
		{
			var info = new DataModelPropertyInfo
			{
				PropertyOrFieldName = fieldInfo.Name,
				PropertyOrFieldType = fieldInfo.FieldType,
			};

			UpdateSPFieldInfo(fieldInfo, info);
			UpdateDefaultValue(fieldInfo, info);
			RegisterCustomConverter(info);

			AddMappingInfo(info);
		}

		private void AddMappingInfo(PropertyInfo propertyInfo)
		{
			var info = new DataModelPropertyInfo
			{
				PropertyOrFieldName = propertyInfo.Name,
				PropertyOrFieldType = propertyInfo.PropertyType,
			};

			UpdateSPFieldInfo(propertyInfo, info);
			UpdateDefaultValue(propertyInfo, info);
			RegisterCustomConverter(info);

			AddMappingInfo(info);
		}

		private void AddMappingInfo(DataModelPropertyInfo info)
		{
			_mappings.Add(info.PropertyOrFieldName, info);
		}

		private static void UpdateSPFieldInfo(MemberInfo memberInfo, DataModelPropertyInfo info)
		{
			var fieldAttribute = memberInfo.GetCustomAttribute<SpFieldAttribute>();
			if (fieldAttribute == null) return;

			info.SpFieldInternalName = fieldAttribute.InternalName ?? info.PropertyOrFieldName;
			info.CustomConverterType = fieldAttribute.CustomConverterType;
		}

		private static void UpdateDefaultValue(MemberInfo memberInfo, DataModelPropertyInfo info)
		{
			var defaultAttribute = memberInfo.GetCustomAttribute<DefaultValueAttribute>();
			if (defaultAttribute == null) return;

			info.DefaultValue = defaultAttribute.Value;
		}

		private static void RegisterCustomConverter(DataModelPropertyInfo info)
		{
			if (info.CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(info.CustomConverterType);
			}
		}

		#endregion
	}
}