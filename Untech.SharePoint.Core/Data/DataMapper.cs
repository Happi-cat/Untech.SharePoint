using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Data.Converters;

namespace Untech.SharePoint.Core.Data
{
	internal class DataMapper
	{
		public DataMapper(DataModel model)
		{
			DataModel = model;
		}

		public DataModel DataModel { get; set; }

		public void Map(SPListItem sourceItem, object destItem)
		{
			if (!DataModel.ModelType.IsInstanceOfType(destItem))
			{
				throw new ArgumentException("destItem");
			}

			var fields = sourceItem.Fields;
			foreach (var mappingInfo in DataModel.PropertyInfos)
			{
				var field = fields.GetField(mappingInfo.SpFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field);
			}
		}

		public void Map(object sourceItem, SPListItem destItem)
		{
			if (!DataModel.ModelType.IsInstanceOfType(sourceItem))
			{
				throw new ArgumentException("sourceItem");
			}

			var fields = destItem.Fields;
			foreach (var mappingInfo in DataModel.PropertyInfos)
			{
				var field = fields.GetField(mappingInfo.SpFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field);
			}
		}

		private static IFieldConverter GetConverter(DataModelPropertyInfo info, SPField field)
		{
			var converter = info.CustomConverterType != null ?
				FieldConverterResolver.Instance.Create(info.CustomConverterType) :
				FieldConverterResolver.Instance.Create(field.TypeAsString);

			converter.Initialize(field, info.PropertyOrFieldType);

			return converter;
		}

		private void MapProperty(SPListItem sourceItem, object destItem, DataModelPropertyInfo info, SPField field)
		{
			try
			{
				var converter = GetConverter(info, field);

				var spValue = sourceItem[field.Id];
				if (spValue == null && info.DefaultValue != null)
				{
					DataModel.PropertyAccessor[destItem, info.PropertyOrFieldName] = info.DefaultValue;
				}
				else
				{
					var propValue = converter.FromSpValue(spValue);

					DataModel.PropertyAccessor[destItem, info.PropertyOrFieldName] = propValue;
				}
			}
			catch (Exception e)
			{
				throw new PropertyMappingException(info, e);
			}
		}

		private void MapProperty(object sourceItem, SPListItem destItem, DataModelPropertyInfo info, SPField field)
		{
			if (field.ReadOnlyField)
			{
				return;
			}

			try
			{
				var converter = GetConverter(info, field);

				var propValue = DataModel.PropertyAccessor[sourceItem, info.PropertyOrFieldName];
				var spValue = converter.ToSpValue(propValue);

				destItem[field.Id] = spValue;
			}
			catch (Exception e)
			{
				throw new PropertyMappingException(info, e);
			}
		}
	}
}
