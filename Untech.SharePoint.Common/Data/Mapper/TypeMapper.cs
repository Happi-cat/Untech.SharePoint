using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Data.Mapper
{
	public abstract class TypeMapper<TSPItem>
	{
		protected TypeMapper(MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
			TypeCreator = InstanceCreationUtility.GetCreator<object>(contentType.EntityType);
		}

		public MetaContentType ContentType { get; private set; }

		public Func<object> TypeCreator { get; private set; }

		public virtual void Map(object source, TSPItem dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			foreach (var mapper in GetMappers())
			{
				mapper.Map(source, dest);
			}

			SetContentType(dest);
		}

		public virtual void Map(TSPItem source, object dest, IReadOnlyCollection<string> viewFields = null)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			foreach (var mapper in GetMappers(viewFields))
			{
				mapper.Map(source, dest);
			}
		}

		public virtual object CreateAndMap(TSPItem source, IReadOnlyCollection<string> viewFields = null)
		{
			var item = TypeCreator();

			Map(source, item, viewFields);

			return item;
		}

		protected abstract void SetContentType(TSPItem spItem);

		protected IEnumerable<FieldMapper<TSPItem>> GetMappers()
		{
			return ContentType.Fields
				.Select<MetaField, FieldMapper<TSPItem>>(n => n.GetMapper<TSPItem>());
		}

		protected IEnumerable<FieldMapper<TSPItem>> GetMappers(IReadOnlyCollection<string> viewFields)
		{
			return ContentType.Fields
				.Where<MetaField>(n => viewFields.IsNullOrEmpty() || n.InternalName.In(viewFields))
				.Select(n => n.GetMapper<TSPItem>());
		}
	}
}