using System.Collections.Generic;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	internal class ModelConverters
	{
		public Dictionary<string, IFieldConverter> Converters { get; private set; }
		public MetaModel Model { get; private set; }

		public ModelConverters(MetaModel model, SPFieldCollection fields)
		{
			Guard.ThrowIfArgumentNull(model, "model");
			Guard.ThrowIfArgumentNull(fields, "fields");

			Model = model;
			Converters = new Dictionary<string, IFieldConverter>();

			foreach (var metaProperty in model.MetaProperties)
			{
				var field = fields.GetField(metaProperty.SpFieldInternalName);
				Converters.Add(metaProperty.MemberName, InstantiateConverter(metaProperty, field));
			}
		}

		public IFieldConverter this[string memberName]
		{
			get { return Converters[memberName]; }
		}

		private static IFieldConverter InstantiateConverter(MetaProperty info, SPField field)
		{
			var converter = info.CustomConverterType != null ?
				FieldConverterResolver.Instance.Create(info.CustomConverterType) :
				FieldConverterResolver.Instance.Create(field.TypeAsString);

			converter.Initialize(field, info.MemberType);

			return converter;
		}
	}
}