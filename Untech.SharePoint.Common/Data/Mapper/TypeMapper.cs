using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Data.Mapper
{
	public abstract class TypeMapper<TListItem>
	{
		protected TypeMapper(MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
			TypeCreator = InstanceCreationUtility.GetCreator<object>(contentType.EntityType);
		}

		public MetaContentType ContentType { get; private set; }

		public Func<object> TypeCreator { get; private set; }

		public virtual void Map(object source, TListItem dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			foreach (var mapper in GetMappers())
			{
				mapper.Map(source, dest);
			}
		}

		public virtual void Map(TListItem source, object dest, IReadOnlyCollection<string> viewFields = null)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			foreach (var mapper in GetMappers(viewFields))
			{
				mapper.Map(source, dest);
			}
		}

		protected abstract IEnumerable<FieldMapper<TListItem>>  GetMappers();

		protected abstract IEnumerable<FieldMapper<TListItem>> GetMappers(IReadOnlyCollection<string> viewFields);
	}
}