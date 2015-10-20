using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.MetaModels;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class TypeMapper
	{
		public TypeMapper(MetaContentType contentType)
		{
			Common.Utils.Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
			TypeCreator = InstanceCreationUtility.GetCreator<object>(contentType.EntityType);
		}

		public MetaContentType ContentType { get; private set; }

		public Func<object> TypeCreator { get; private set; }

		public void Map(object source, ListItem dest)
		{
			Common.Utils.Guard.CheckNotNull("source", source);
			Common.Utils.Guard.CheckNotNull("dest", dest);

			var mappers = ContentType.Fields.Select<MetaField, FieldMapper>(n => n.GetMapper());

			foreach (var mapper in mappers)
			{
				mapper.Map(source, dest);
			}
		}

		public void Map(ListItem source, object dest)
		{
			Common.Utils.Guard.CheckNotNull("source", source);
			Common.Utils.Guard.CheckNotNull("dest", dest);

			var mappers = ContentType.Fields.Select<MetaField, FieldMapper>(n => n.GetMapper());

			foreach (var mapper in mappers)
			{
				mapper.Map(source, dest);
			}
		}
	}
}