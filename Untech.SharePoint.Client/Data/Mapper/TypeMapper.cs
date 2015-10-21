using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.MetaModels;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class TypeMapper : TypeMapper<ListItem>
	{
		public TypeMapper(MetaContentType contentType)
			: base(contentType)
		{

		}

		
		protected override IEnumerable<FieldMapper<ListItem>> GetMappers()
		{
			return ContentType.Fields
				.Select<MetaField, FieldMapper>(n => n.GetMapper());
		}

		protected override IEnumerable<FieldMapper<ListItem>> GetMappers(IReadOnlyCollection<string> viewFields)
		{
			return ContentType.Fields
				.Where<MetaField>(n => viewFields.IsNullOrEmpty() || n.InternalName.In(viewFields))
				.Select(n => n.GetMapper());
		}
	}
}