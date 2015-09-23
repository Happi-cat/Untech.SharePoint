using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Visitors
{
	public class FieldConverterCreator : BaseMetaModelVisitor
	{
		public FieldConverterCreator(IFieldConverterResolver resolver)
		{
			Resolver = resolver;
		}

		protected IFieldConverterResolver Resolver { get; private set; }

		public override void VisitField(MetaField field)
		{
			SetConverter(field,
				field.CustomConverterType != null
					? Resolver.Resolve(field.CustomConverterType)
					: Resolver.Resolve(field.TypeAsString));
		}

		private void SetConverter(MetaField field, IFieldConverter converter)
		{
			converter.Initialize(field);

		}
	}
}