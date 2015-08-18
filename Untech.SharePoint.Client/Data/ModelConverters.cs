using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.FieldConverters;

namespace Untech.SharePoint.Client.Data
{
	internal class ModelConverters
	{
		public Dictionary<string, IFieldConverter> Converters { get; private set; }
		public MetaModel Model { get; private set; }

		public ModelConverters(MetaModel model, IList<Field> fields)
		{
			Guard.CheckNotNull("model", model);
			Guard.CheckNotNull("fields", fields);

			Model = model;
			Converters = new Dictionary<string, IFieldConverter>();

			foreach (var metaProperty in model.MetaProperties)
			{
				var field = fields.Single(n => n.InternalName == metaProperty.SpFieldInternalName);
				Converters.Add(metaProperty.MemberName, InstantiateConverter(metaProperty, field));
			}
		}

		public IFieldConverter this[string memberName]
		{
			get { return Converters[memberName]; }
		}

		private static IFieldConverter InstantiateConverter(MetaProperty info, Field field)
		{
			var converter = info.CustomConverterType != null ?
				FieldConverterResolver.Instance.Create(info.CustomConverterType) :
				FieldConverterResolver.Instance.Create(field.TypeAsString);

			converter.Initialize(field, info.MemberType);

			return converter;
		}
	}
}