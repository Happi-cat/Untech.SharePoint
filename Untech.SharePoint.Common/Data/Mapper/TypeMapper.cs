using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Data.Mapper
{
	/// <summary>
	/// Represents class that can map SP List item to Entity.
	/// </summary>
	/// <typeparam name="TSPItem">Exact type of SP list item, i.e. SPListItem for SSOM, ListItem for CSOM.</typeparam>
	[PublicAPI]
	public abstract class TypeMapper<TSPItem>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMapper{TSPItem}"/> for the specified SP ContentType.
		/// </summary>
		/// <param name="contentType">ContentType to map.</param>
		/// <exception cref="ArgumentNullException"><paramref name="contentType"/> is null.</exception>
		protected TypeMapper([NotNull]MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
			TypeCreator = InstanceCreationUtility.GetCreator<object>(contentType.EntityType);
		}

		/// <summary>
		/// Gets assocaited SP ContentType metadata.
		/// </summary>
		[NotNull]
		public MetaContentType ContentType { get; private set; }

		/// <summary>
		/// Gets .NET type creator.
		/// </summary>
		[NotNull]
		public Func<object> TypeCreator { get; private set; }

		/// <summary>
		/// Maps source entity to SP list item.
		/// </summary>
		/// <param name="source">Source object.</param>
		/// <param name="dest">Destination SP list item.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dest"/> is null.</exception>
		public void Map([NotNull]object source, [NotNull]TSPItem dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			foreach (var mapper in GetMappers())
			{
				mapper.Map(source, dest);
			}

			SetContentType(dest);
		}

		/// <summary>
		/// Maps SP list item to destination entity.
		/// </summary>
		/// <param name="source">Souce SP list item to map.</param>
		/// <param name="dest">Destination object.</param>
		/// <param name="viewFields">Collection of fields internal names that should be mapped.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dest"/> is null.</exception>
		public void Map([NotNull]TSPItem source, [NotNull]object dest, IReadOnlyCollection<MemberRefModel> viewFields = null)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			foreach (var mapper in GetMappers(viewFields))
			{
				mapper.Map(source, dest);
			}
		}

		/// <summary>
		/// Create and maps SP list item to .NET entity.
		/// </summary>
		/// <param name="source">Souce SP list item to map.</param>
		/// <param name="viewFields">Collection of fields internal names that should be mapped.</param>
		/// <returns>New .NET entity.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public object CreateAndMap([NotNull]TSPItem source, IReadOnlyCollection<MemberRefModel> viewFields = null)
		{
			Guard.CheckNotNull("source", source);

			var item = TypeCreator();

			Map(source, item, viewFields);

			return item;
		}

		/// <summary>
		/// Sets current contnet type id for the specified SP list item.
		/// </summary>
		/// <param name="spItem">SP list item</param>
		protected abstract void SetContentType([NotNull]TSPItem spItem);

		/// <summary>
		/// Gets collection of field mappers associated with current <see cref="ContentType"/>.
		/// </summary>
		/// <returns>Collection of field mappers.</returns>
		[NotNull]
		protected IEnumerable<FieldMapper<TSPItem>> GetMappers()
		{
			return ContentType.Fields
				.Select<MetaField, FieldMapper<TSPItem>>(n => n.GetMapper<TSPItem>());
		}

		/// <summary>
		/// Gets collection of field mappers associated with current <see cref="ContentType"/> and filtered in according to list of selectable fields.
		/// </summary>
		/// <returns>Collection of field mappers for fields that should be mapped.</returns>
		[NotNull]
		protected IEnumerable<FieldMapper<TSPItem>> GetMappers([CanBeNull]IReadOnlyCollection<MemberRefModel> viewFields)
		{
			if (viewFields.IsNullOrEmpty())
			{
				return GetMappers();
			}

			var viewMembers = viewFields.Select(n => n.Member).ToList();

			return ContentType.Fields
				.Where<MetaField>(n =>  viewMembers.Contains(n.Member))
				.Select(n => n.GetMapper<TSPItem>());
		}
	}
}