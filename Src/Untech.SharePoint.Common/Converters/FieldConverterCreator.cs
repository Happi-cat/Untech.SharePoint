using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters
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

		private IFieldConverterResolver Resolver { get; }

		/// <summary>
		/// Visit <see cref="MetaField"/>
		/// </summary>
		/// <param name="field">Field to visit.</param>
		public override void VisitField(MetaField field)
		{
			var converter = CreateConverter(field);
			converter.Initialize(field);
			field.Converter = converter;
		}

		[NotNull]
		private IFieldConverter CreateConverter(MetaField field)
		{
			if (field.CustomConverterType != null)
			{
				if (!Resolver.CanResolve(field.CustomConverterType))
				{
					throw Error.ConverterNotFound(field.CustomConverterType);
				}
				return Resolver.Resolve(field.CustomConverterType);
			}

			if (!Resolver.CanResolve(field.TypeAsString))
			{
				throw Error.ConverterNotFound(field.TypeAsString);
			}

			return Resolver.Resolve(field.TypeAsString);
		}
	}
}