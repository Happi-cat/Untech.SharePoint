using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Mappings;

namespace Untech.SharePoint.Common.Configuration
{
	public class Config
	{
		protected internal virtual IFieldConverterResolver FieldConverters { get; set; }

		protected internal virtual IMappingSourceResolver Mappings { get; set; }
	}
}