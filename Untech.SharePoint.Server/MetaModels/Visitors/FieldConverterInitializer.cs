using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Server.MetaModels.Visitors
{
	internal class FieldConverterInitializer : BaseMetaModelVisitor
	{
		public FieldConverterInitializer(IFieldConverterResolver converterResolver)
		{
			ConverterResolver = converterResolver;
		}

		public IFieldConverterResolver ConverterResolver { get; private set; }

		public override void VisitField(MetaField field)
		{
			var converter = field.CustomConverterType != null
				? ConverterResolver.Resolve(field.CustomConverterType)
				: ConverterResolver.Resolve(field.TypeAsString);

			InitializeConverter(field, converter);
		}

		private void InitializeConverter(MetaField field, IFieldConverter converter)
		{
			converter.Initialize(field);

			field.Converter = converter;
		}
	}
}