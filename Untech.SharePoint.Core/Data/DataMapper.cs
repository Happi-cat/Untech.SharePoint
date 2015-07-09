using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Data.Converters;

namespace Untech.SharePoint.Core.Data
{
	internal class DataMapper
	{
		public DataMapper(MetaModel model)
		{
			MetaModel = model;
		}

		internal DataMapper(MetaModel model, ModelConverters modelConverters)
		{
			Guard.ThrowIfArgumentNull(model, "model");
			Guard.ThrowIfArgumentNull(modelConverters, "modelConverters");

			if (modelConverters.Model != model)
			{
				throw new ArgumentException("Invalid model converters");
			}

			MetaModel = model;
			ModelConverters = modelConverters;
		}

		public MetaModel MetaModel { get; private set; }
		private ModelConverters ModelConverters { get; set; }

		public void Map(SPListItem sourceItem, object destItem)
		{
			if (!MetaModel.ModelType.IsInstanceOfType(destItem))
			{
				throw new ArgumentException("destItem");
			}

			var fields = sourceItem.Fields;
			var converters = GetConverters(fields);
			foreach (var mappingInfo in MetaModel.MetaProperties)
			{
				var field = fields.GetField(mappingInfo.SpFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field, converters);
			}
		}

		public void Map(object sourceItem, SPListItem destItem)
		{
			if (!MetaModel.ModelType.IsInstanceOfType(sourceItem))
			{
				throw new ArgumentException("sourceItem");
			}

			var fields = destItem.Fields;
			var converters = GetConverters(fields);
			foreach (var mappingInfo in MetaModel.MetaProperties)
			{
				var field = fields.GetField(mappingInfo.SpFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field, converters);
			}
		}

		private ModelConverters GetConverters(SPFieldCollection fields)
		{
			return ModelConverters ?? new ModelConverters(MetaModel, fields);
		}

		private void MapProperty(SPListItem sourceItem, object destItem, MetaProperty info, SPField field, ModelConverters converters)
		{
			try
			{
				var converter = converters[info.MemberName];

				var spValue = sourceItem[field.Id];
				if (spValue == null && info.DefaultValue != null)
				{
					MetaModel.PropertyAccessor[destItem, info.MemberName] = info.DefaultValue;
				}
				else
				{
					var propValue = converter.FromSpValue(spValue);

					MetaModel.PropertyAccessor[destItem, info.MemberName] = propValue;
				}
			}
			catch (Exception e)
			{
				throw new PropertyMappingException(info, e);
			}
		}

		private void MapProperty(object sourceItem, SPListItem destItem, MetaProperty info, SPField field, ModelConverters converters)
		{
			if (field.ReadOnlyField)
			{
				return;
			}

			try
			{
				var converter = converters[info.MemberName];

				var propValue = MetaModel.PropertyAccessor[sourceItem, info.MemberName];
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
