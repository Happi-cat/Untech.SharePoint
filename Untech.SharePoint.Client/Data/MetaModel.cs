using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.FieldConverters;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class MetaModel
	{
		public MetaModel(MetaType metaType, IReadOnlyCollection<Field> fields)
		{
			Type = metaType;
			SpFields = fields;

			InitConverters();
		}

		public MetaType Type { get; private set; }

		public IReadOnlyCollection<Field> SpFields { get; private set; }

		public IReadOnlyDictionary<string, IFieldConverter> Converters { get; private set; }

		private void InitConverters()
		{
			Converters = Type.DataMembers.ToDictionary(n => n.Name, CreateConverter);
		}

		private IFieldConverter CreateConverter(MetaDataMember member)
		{
			var spField = SpFields.Single(n => n.InternalName == member.SpFieldInternalName);

			var converter = member.CustomConverterType == null
				? FieldConverterResolver.Instance.Create(spField.TypeAsString)
				: FieldConverterResolver.Instance.Create(member.CustomConverterType);

			converter.Initialize(spField, member.Type);

			return converter;
		}
	}
}