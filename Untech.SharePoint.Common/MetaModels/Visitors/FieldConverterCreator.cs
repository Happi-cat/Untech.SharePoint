using Untech.SharePoint.Common.Converters;

namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	/// <summary>
	/// Represents class that will instantiate <see cref="MetaField.Converter"/> for all <see cref="MetaField"/>.
	/// </summary>
	public sealed class FieldConverterCreator : BaseMetaModelVisitor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldConverterCreator"/> class with specified resolver.
		/// </summary>
		/// <param name="resolver">Field converter resolver.</param>
		public FieldConverterCreator(IFieldConverterResolver resolver)
		{
			Resolver = resolver;
		}

		private IFieldConverterResolver Resolver { get; set; }

		/// <summary>
		/// Visit <see cref="MetaField"/>
		/// </summary>
		/// <param name="field">Field to visit.</param>
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