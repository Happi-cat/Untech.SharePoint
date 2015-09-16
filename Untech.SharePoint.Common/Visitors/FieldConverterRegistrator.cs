using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Visitors
{
	public class FieldConverterRegistrator : BaseMetaModelVisitor
	{
		public FieldConverterRegistrator(FieldConvertersConfiguration config)
		{
			Guard.CheckNotNull("config", config);

			Configuration = config;
		}

		public FieldConvertersConfiguration Configuration { get; private set; }

		public override void VisitField(MetaField field)
		{
			if (field.CustomConverterType != null)
			{
				Configuration.Register(field.CustomConverterType);
			}
		}
	}
}