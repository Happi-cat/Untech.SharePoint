using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data.QueryModels;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.Utils;
using Untech.SharePoint.Utils.Reflection;

namespace Untech.SharePoint.Data.Mapper
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
			Guard.CheckNotNull(nameof(contentType), contentType);

			ContentType = contentType;
			TypeCreator = InstanceCreationUtility.GetCreator<object>(contentType.EntityType);
		}

		/// <summary>
		/// Gets associated SP ContentType meta-data.
		/// </summary>
		[NotNull]
		public MetaContentType ContentType { get; }

		/// <summary>
		/// Gets .NET type creator.
		/// </summary>
		[NotNull]
		public Func<object> TypeCreator { get; }

		/// <summary>
		/// Maps source entity to SP list item.
		/// </summary>
		/// <param name="source">Source object.</param>
		/// <param name="dest">Destination SP list item.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dest"/> is null.</exception>
		public void Map([NotNull]object source, [NotNull]TSPItem dest)
		{
			Guard.CheckNotNull(nameof(source), source);
			Guard.CheckNotNull(nameof(dest), dest);

			foreach (var mapper in GetMappers())
			{
				mapper.Map(source, dest);
			}

			if (ContentType.List.IsExternal)
			{
				return;
			}

			SetContentType(dest);
		}

		/// <summary>
		/// Maps source entity to dictionary with CAML values.
		/// </summary>
		/// <param name="source">Source object.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public IReadOnlyDictionary<string, string> MapToCaml([NotNull] object source)
		{
			Guard.CheckNotNull(nameof(source), source);

			var fields = new Dictionary<string, string>();

			var mappers = GetMappers()
				.Where(n => n.StoreAccessor.CanSetValue || n.Field.InternalName.In(new[] { Fields.Id, Fields.BdcIdentity }));

			foreach (var mapper in mappers)
			{
				fields[mapper.Field.InternalName] = mapper.MapToCaml(source);
			}

			if (ContentType.List.IsExternal)
			{
				return fields;
			}

			fields[Fields.ContentTypeId] = ContentType.Id;

			return fields;
		}

		/// <summary>
		/// Maps SP list item to destination entity.
		/// </summary>
		/// <param name="source">Source SP list item to map.</param>
		/// <param name="dest">Destination object.</param>
		/// <param name="viewFields">Collection of fields internal names that should be mapped.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dest"/> is null.</exception>
		public void Map([NotNull]TSPItem source, [NotNull]object dest, IReadOnlyCollection<MemberRefModel> viewFields = null)
		{
			Guard.CheckNotNull(nameof(source), source);
			Guard.CheckNotNull(nameof(dest), dest);

			foreach (var mapper in GetMappers(viewFields))
			{
				mapper.Map(source, dest);
			}
		}

		/// <summary>
		/// Creates and maps SP list item to .NET entity.
		/// </summary>
		/// <param name="source">Source SP list item to map.</param>
		/// <param name="viewFields">Collection of fields internal names that should be mapped.</param>
		/// <returns>New .NET entity.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public object CreateAndMap([NotNull]TSPItem source, IReadOnlyCollection<MemberRefModel> viewFields = null)
		{
			Guard.CheckNotNull(nameof(source), source);

			var item = TypeCreator();

			Map(source, item, viewFields);

			return item;
		}

		/// <summary>
		/// Creates and maps SP list items to .NET entities.
		/// </summary>
		/// <param name="source">Source SP list items to map.</param>
		/// <param name="viewFields">Collection of fields internal names that should be mapped.</param>
		/// <returns>New .NET entities.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public IEnumerable<T> CreateAndMap<T>([NotNull]IEnumerable<TSPItem> source, IReadOnlyCollection<MemberRefModel> viewFields = null)
		{
			Guard.CheckNotNull(nameof(source), source);
			var mappers = GetMappers(viewFields).ToList();

			foreach (var spItem in source)
			{
				var item = TypeCreator();
				foreach (var mapper in mappers)
				{
					mapper.Map(spItem, item);
				}
				yield return (T)item;
			}
		}

		/// <summary>
		/// Sets current content type id for the specified SP list item.
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
			foreach (var field in (IEnumerable<MetaField>)ContentType.Fields)
			{
				yield return field.GetMapper<TSPItem>();
			}
		}

		/// <summary>
		/// Gets collection of field mappers associated with current <see cref="ContentType"/> and filtered in according to list of selectable fields.
		/// </summary>
		/// <returns>Collection of field mappers for fields that should be mapped.</returns>
		[NotNull]
		protected IEnumerable<FieldMapper<TSPItem>> GetMappers([CanBeNull]IReadOnlyCollection<MemberRefModel> viewFields)
		{
			return viewFields.IsNullOrEmpty()
				? GetMappers()
				: GetMappers(viewFields.Select(n => n.Member).ToList());
		}

		private IEnumerable<FieldMapper<TSPItem>> GetMappers([NotNull]IReadOnlyCollection<MemberInfo> viewMembers)
		{
			foreach (var field in (IEnumerable<MetaField>)ContentType.Fields)
			{
				if (viewMembers.Contains(field.Member, MemberInfoComparer.Default))
				{
					yield return field.GetMapper<TSPItem>();
				}
			}
		}
	}
}