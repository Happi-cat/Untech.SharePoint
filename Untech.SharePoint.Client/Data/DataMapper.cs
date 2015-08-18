using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	internal class DataMapper
	{
		public DataMapper(MetaModel model)
		{
			MetaModel = model;
		}

		internal DataMapper(MetaModel model, ModelConverters modelConverters)
		{
			Guard.CheckNotNull("model", model);
			Guard.CheckNotNull("modelConverters", modelConverters);

			if (modelConverters.Model != model)
			{
				throw new ArgumentException("Invalid model converters");
			}

			MetaModel = model;
			ModelConverters = modelConverters;
		}

		public MetaModel MetaModel { get; private set; }
		private ModelConverters ModelConverters { get; set; }

		public void Map(ListItem sourceItem, object destItem, IList<Field> fields)
		{
			if (!MetaModel.ModelType.IsInstanceOfType(destItem))
			{
				throw new ArgumentException("destItem");
			}

			var converters = GetConverters(fields);
			foreach (var mappingInfo in MetaModel.MetaProperties)
			{
				var field = fields.First(n=> n.InternalName == mappingInfo.SpFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field, converters);
			}
		}

		public void Map(object sourceItem, ListItem destItem, IList<Field> fields)
		{
			if (!MetaModel.ModelType.IsInstanceOfType(sourceItem))
			{
				throw new ArgumentException("sourceItem");
			}

			var converters = GetConverters(fields);
			foreach (var mappingInfo in MetaModel.MetaProperties)
			{
				var field = fields.First(n => n.InternalName == mappingInfo.SpFieldInternalName);
				MapProperty(sourceItem, destItem, mappingInfo, field, converters);
			}
		}

		private ModelConverters GetConverters(IList<Field> fields)
		{
			return ModelConverters ?? new ModelConverters(MetaModel, fields);
		}

		private void MapProperty(ListItem sourceItem, object destItem, MetaProperty info, Field field, ModelConverters converters)
		{
			try
			{
				var converter = converters[info.MemberName];

				var spValue = sourceItem[field.InternalName];
				if (spValue == null && info.DefaultValue != null)
				{
					MetaModel.PropertyAccessor[destItem, info.MemberName] = info.DefaultValue;
				}
				else
				{
					var propValue = converter.FromClientValue(spValue);

					MetaModel.PropertyAccessor[destItem, info.MemberName] = propValue;
				}
			}
			catch (Exception e)
			{
				throw new PropertyMappingException(info, e);
			}
		}

		private void MapProperty(object sourceItem, ListItem destItem, MetaProperty info, Field field, ModelConverters converters)
		{
			if (field.ReadOnlyField)
			{
				return;
			}

			try
			{
				var converter = converters[info.MemberName];

				var propValue = MetaModel.PropertyAccessor[sourceItem, info.MemberName];
				var spValue = converter.ToClientValue(propValue);

				destItem[field.InternalName] = spValue;
			}
			catch (Exception e)
			{
				throw new PropertyMappingException(info, e);
			}
		}
	}
}
