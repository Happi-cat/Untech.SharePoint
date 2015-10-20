using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Mappings;

namespace Untech.SharePoint.Common.Configuration
{
	public class Config
	{
		public virtual IFieldConverterResolver FieldConverters { get; set; }

		public virtual IMappingSourceResolver Mappings { get; set; }
	}
}