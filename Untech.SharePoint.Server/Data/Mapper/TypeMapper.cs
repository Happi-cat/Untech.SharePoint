using System;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Common.Utils.Reflection;
using Untech.SharePoint.Server.MetaModels;

namespace Untech.SharePoint.Server.Data.Mapper
{
	internal sealed class TypeMapper
	{
		public TypeMapper(MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
			TypeCreator = InstanceCreationUtility.GetCreator<object>(contentType.EntityType);
		}

		public MetaContentType ContentType { get; private set; }

		public Func<object> TypeCreator { get; private set; }

		public void Map(object source, SPListItem dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			ContentType.Fields
				.Select<MetaField, FieldMapper>(n => n.GetMapper())
				.ToList()
				.ForEach(n => n.Map(source, dest));
		}

		public void Map(SPListItem source, object dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			ContentType.Fields
				.Select<MetaField,FieldMapper>(n => n.GetMapper())
				.ToList()
				.ForEach(n => n.Map(source, dest));
		}
	}
}