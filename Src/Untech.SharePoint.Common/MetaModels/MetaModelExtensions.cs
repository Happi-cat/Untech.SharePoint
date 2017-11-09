using System;
using System.Linq;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data.Mapper;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.MetaModels
{
	/// <summary>
	/// Provides a set of static methods for easy work with <see cref="IMetaModel"/> additional properties.
	/// </summary>
	public static class MetaModelExtensions
	{
		private const string MapperProperty = "Mapper";

		/// <summary>
		/// Gets field mapper for the specified <paramref name="field"/>
		/// </summary>
		/// <typeparam name="TSPItem">Exact SP list item type, i.e. SPListItem for SSOM, or ListItem for CSOM.</typeparam>
		/// <param name="field">Current field.</param>
		/// <returns>Instance of <see cref="FieldMapper{TSPItem}"/> of the specified <see cref="MetaField"/></returns>
		/// <exception cref="ArgumentNullException"><paramref name="field"/> is null.</exception>
		public static FieldMapper<TSPItem> GetMapper<TSPItem>([NotNull]this MetaField field)
		{
			Guard.CheckNotNull(nameof(field), field);

			return field.GetAdditionalProperty<FieldMapper<TSPItem>>(MapperProperty);
		}

		/// <summary>
		/// Gets type mapper for the specified <paramref name="contentType"/>
		/// </summary>
		/// <typeparam name="TSPItem">Exact SP list item type, i.e. SPListItem for SSOM, or ListItem for CSOM.</typeparam>
		/// <param name="contentType">Current content type.</param>
		/// <returns>Instance of <see cref="TypeMapper{TSPItem}"/> of the current <see cref="MetaContentType"/></returns>
		/// <exception cref="ArgumentNullException"><paramref name="contentType"/> is null.</exception>
		public static TypeMapper<TSPItem> GetMapper<TSPItem>([NotNull]this MetaContentType contentType)
		{
			Guard.CheckNotNull(nameof(contentType), contentType);

			return contentType.GetAdditionalProperty<TypeMapper<TSPItem>>(MapperProperty);
		}

		/// <summary>
		/// Sets field mapper for the specified <paramref name="field"/>
		/// </summary>
		/// <typeparam name="TSPItem">Exact SP list item type, i.e. SPListItem for SSOM, or ListItem for CSOM.</typeparam>
		/// <param name="field">Current field.</param>
		/// <param name="mapper">Instance of <see cref="FieldMapper{TSPItem}"/> that should be associated with specified <see cref="MetaField"/></param>
		/// <exception cref="ArgumentNullException"><paramref name="field"/> or <paramref name="mapper"/> is null.</exception>
		public static void SetMapper<TSPItem>([NotNull]this MetaField field, [NotNull]FieldMapper<TSPItem> mapper)
		{
			Guard.CheckNotNull(nameof(field), field);
			Guard.CheckNotNull(nameof(mapper), mapper);

			field.SetAdditionalProperty(MapperProperty, mapper);
		}

		/// <summary>
		/// Sets type mapper for the specified <paramref name="contentType"/>
		/// </summary>
		/// <typeparam name="TSPItem">Exact SP list item type, i.e. SPListItem for SSOM, or ListItem for CSOM.</typeparam>
		/// <param name="contentType">Current content type.</param>
		/// <param name="mapper">Instance of <see cref="TypeMapper{TSPItem}"/> that should be associated with specified <see cref="MetaContentType"/></param>
		/// <exception cref="ArgumentNullException"><paramref name="contentType"/> or <paramref name="mapper"/> is null.</exception>
		public static void SetMapper<TSPItem>([NotNull]this MetaContentType contentType, [NotNull]TypeMapper<TSPItem> mapper)
		{
			Guard.CheckNotNull(nameof(contentType), contentType);
			Guard.CheckNotNull(nameof(mapper), mapper);

			contentType.SetAdditionalProperty(MapperProperty, mapper);
		}

		/// <summary>
		/// Finds key field of the content type.
		/// </summary>
		/// <param name="contentType">Content type to get key field.</param>
		/// <returns>Meta-data of the key field or null.</returns>
		[CanBeNull]
		public static MetaField GetKeyField([NotNull]this MetaContentType contentType)
		{
			Guard.CheckNotNull(nameof(contentType), contentType);

			return contentType.List.IsExternal
				? contentType.Fields.FirstOrDefault<MetaField>(n => n.InternalName == Fields.BdcIdentity)
				: contentType.Fields.FirstOrDefault<MetaField>(n => n.InternalName == Fields.Id);
		}
	}
}