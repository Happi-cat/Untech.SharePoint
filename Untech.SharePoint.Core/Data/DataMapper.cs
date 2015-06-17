using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Data.Converters;
using Untech.SharePoint.Core.Reflection;

namespace Untech.SharePoint.Core.Data
{
	internal class DataMapper
	{
		private readonly PropertyAccessor _propertyAccessor = new PropertyAccessor();
		private readonly PropertyMappings _propertyMappings = new PropertyMappings();
		private Type _initializedType;

		public void Initialize(Type objectType)
		{
			_initializedType = objectType;
			_propertyAccessor.Initialize(objectType);
			_propertyMappings.Initialize(objectType);
		}

		public void Map(SPListItem sourceItem, object destItem)
		{
			if (!_initializedType.IsInstanceOfType(destItem))
			{
				throw new ArgumentException("destItem");
			}

			var fields = sourceItem.Fields;
			foreach (var mappingInfo in _propertyMappings)
			{
				var field = fields.GetField(mappingInfo.SPFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field);
			}
		}

		public void Map(object sourceItem, SPListItem destItem)
		{
			if (!_initializedType.IsInstanceOfType(sourceItem))
			{
				throw new ArgumentException("sourceItem");
			}

			var fields = destItem.Fields;
			foreach (var mappingInfo in _propertyMappings)
			{
				var field = fields.GetField(mappingInfo.SPFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field);
			}
		}

		private IFieldConverter GetConverter(PropertyMappingInfo mappingInfo, SPField field)
		{
			IFieldConverter converter = mappingInfo.CustomConverterType != null ?
				FieldConverterResolver.Instance.Create(mappingInfo.CustomConverterType) :
				FieldConverterResolver.Instance.Create(field.TypeAsString);

			converter.Initialize(field, mappingInfo.PropertyOrFieldType);

			return converter;
		}

		private void MapProperty(SPListItem sourceItem, object destItem, PropertyMappingInfo mappingInfo, SPField field)
		{
			try
			{
				var converter = GetConverter(mappingInfo, field);

				var spValue = sourceItem[field.Id];
				var propValue = converter.FromSpValue(spValue) ?? mappingInfo.DefaultValue;

				_propertyAccessor[destItem, mappingInfo.PropertyOrFieldName] = propValue;
			}
			catch (Exception e)
			{
				throw new DataMapperException(mappingInfo, e);
			}
		}

		private void MapProperty(object sourceItem, SPListItem destItem, PropertyMappingInfo mappingInfo, SPField field)
		{
			if (field.ReadOnlyField)
			{
				return;
			}

			try
			{
				var converter = GetConverter(mappingInfo, field);

				var propValue = _propertyAccessor[sourceItem, mappingInfo.PropertyOrFieldName];
				var spValue = converter.ToSpValue(propValue);

				destItem[field.Id] = spValue;
			}
			catch (Exception e)
			{
				throw new DataMapperException(mappingInfo, e);
			}
		}
	}
}
