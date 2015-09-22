using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Mappings;

namespace Untech.SharePoint.Common.Configuration
{
	public class Config
	{
		protected internal virtual FieldConvertersContainer FieldConverters { get; set; }

		protected internal virtual MappingsContainer Mappings { get; set; }
	}
}