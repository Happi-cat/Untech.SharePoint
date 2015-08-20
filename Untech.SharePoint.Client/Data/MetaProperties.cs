using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class MetaProperties : IEnumerable<MetaProperty>
	{
		private readonly Dictionary<string, MetaProperty> _mappings = new Dictionary<string, MetaProperty>();

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

		public MetaProperty this[string memberName]
		{
			get { return _mappings[memberName]; }
		}

		public IEnumerator<MetaProperty> GetEnumerator()
		{
			return _mappings.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region [Private Methods]

		private void AddMappingInfo(FieldInfo fieldInfo)
		{
			_mappings.Add(fieldInfo.Name, new MetaProperty(fieldInfo));
		}

		private void AddMappingInfo(PropertyInfo propertyInfo)
		{
			_mappings.Add(propertyInfo.Name, new MetaProperty(propertyInfo));
		}

		#endregion

		
	}
}