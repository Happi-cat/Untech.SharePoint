using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Server.MetaModels;

namespace Untech.SharePoint.Server.Data.Mapper
{
	internal sealed class TypeMapper : TypeMapper<SPListItem>
	{
		public TypeMapper(MetaContentType contentType)
			:base (contentType)
		{

		}

		protected override IEnumerable<FieldMapper<SPListItem>> GetMappers()
		{
			return ContentType.Fields
				.Select<MetaField, FieldMapper>(n => n.GetMapper());
		}

		protected override IEnumerable<FieldMapper<SPListItem>> GetMappers(IReadOnlyCollection<string> viewFields)
		{
			return  ContentType.Fields
				.Where<MetaField>(n => viewFields.IsNullOrEmpty() || n.InternalName.In(viewFields))
				.Select(n => n.GetMapper());
		}
	}
}