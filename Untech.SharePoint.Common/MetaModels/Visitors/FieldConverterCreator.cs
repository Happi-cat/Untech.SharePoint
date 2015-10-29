using Untech.SharePoint.Common.Converters;

namespace Untech.SharePoint.Common.MetaModels.Visitors
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
			IFieldConverter converter;
			if (field.CustomConverterType != null)
			{
				if (!Resolver.CanResolve(field.CustomConverterType))
				{
					throw new FieldConverterException("Cannot find converter in Config");
				}
				converter = Resolver.Resolve(field.CustomConverterType);
			}
			else
			{
				if (!Resolver.CanResolve(field.TypeAsString))
				{
					throw new FieldConverterException("Cannot find converter in Config");
				}
				converter = Resolver.Resolve(field.TypeAsString);
			}

			InitializeConverter(field, converter);
		}

		private void InitializeConverter(MetaField field, IFieldConverter converter)
		{
			converter.Initialize(field);

			field.Converter = converter;
		}
	}
}