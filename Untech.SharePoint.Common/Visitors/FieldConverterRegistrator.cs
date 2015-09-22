using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Visitors
{
	public class FieldConverterRegistrator : BaseMetaModelVisitor
	{
		public FieldConverterRegistrator(FieldConvertersContainer config)
		{
			Guard.CheckNotNull("config", config);

			Container = config;
		}

		public FieldConvertersContainer Container { get; private set; }

		public override void VisitField(MetaField field)
		{
			if (field.CustomConverterType != null)
			{
				Container.Add(field.CustomConverterType);
			}
		}
	}
}