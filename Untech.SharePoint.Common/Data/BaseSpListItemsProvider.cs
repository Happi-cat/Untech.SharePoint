using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents base class for SP list items provider.
	/// </summary>
	public abstract class BaseSpListItemsProvider<TSPListItem> : ISpListItemsProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSpListItemsProvider{T}" />
		/// </summary>
		/// <param name="list">Meta models of the SP list.</param>
		protected BaseSpListItemsProvider([NotNull] MetaList list)
		{
			Guard.CheckNotNull("list", list);

			List = list;
		}

		/// <summary>
		/// Gets list associated with this instance of the <see cref="BaseSpListItemsProvider{T}"/>.
		/// </summary>
		[NotNull]
		public MetaList List { get; private set; }

		public IEnumerable<T> Fetch<T>(QueryModel query)
		{
			var contentType = List.ContentTypes[typeof(T)];
			UpdateViewFields(query, contentType);
			var viewFields = query.SelectableFields.ToList();
			var caml = ConvertToCamlString(query, contentType);

			return Materialize<T>(FetchInternal(caml), contentType, viewFields);
		}

		public bool Any<T>(QueryModel query)
		{
			var contentType = List.ContentTypes[typeof(T)];
			UpdateViewFields(query, contentType);
			var caml = ConvertToCamlString(query, contentType);

			return FetchInternal(caml).Any();
		}

		public int Count<T>(QueryModel query)
		{
			var contentType = List.ContentTypes[typeof(T)];
			UpdateViewFields(query, contentType);
			var caml = ConvertToCamlString(query, contentType);

			return FetchInternal(caml).Count;
		}

		public T SingleOrDefault<T>(QueryModel query)
		{
			var contentType = List.ContentTypes[typeof(T)];

			query.RowLimit = 2;
			UpdateViewFields(query, contentType);

			var viewFields = query.SelectableFields.ToList();
			var caml = ConvertToCamlString(query, contentType);

			var foundItems = FetchInternal(caml);
			if (foundItems.Count > 1)
			{
				throw Error.MoreThanOneMatch();
			}
			return foundItems.Count == 1
				? Materialize<T>(foundItems[0], contentType, viewFields)
				: default(T);
		}

		public T FirstOrDefault<T>(QueryModel query)
		{
			var contentType = List.ContentTypes[typeof(T)];

			query.RowLimit = 1;
			UpdateViewFields(query, contentType);

			var viewFields = query.SelectableFields.ToList();
			var caml = ConvertToCamlString(query, contentType);

			var foundItems = FetchInternal(caml);
			return foundItems.Count == 1
				? Materialize<T>(foundItems[0], contentType, viewFields)
				: default(T);
		}

		public T ElementAtOrDefault<T>(QueryModel query, int index)
		{
			var contentType = List.ContentTypes[typeof(T)];

			query.RowLimit = index + 1;
			UpdateViewFields(query, contentType);

			var viewFields = query.SelectableFields.ToList();
			var caml = ConvertToCamlString(query, contentType);

			var foundItems = FetchInternal(caml);
			return foundItems.Count == 1
				? Materialize<T>(foundItems[0], contentType, viewFields)
				: default(T);
		}

		public T Get<T>(int id)
		{
			if (List.IsExternal)
			{
				throw Error.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			return Materialize<T>(GetInternal(id, contentType), contentType);
		}

		public T Add<T>(T item)
		{
			if (List.IsExternal)
			{
				throw Error.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw Error.OperationRequireIdField();
			}

			var id = AddInternal(item, contentType);

			return Get<T>(id);
		}

		public void Update<T>(T item)
		{
			if (List.IsExternal)
			{
				throw Error.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw Error.OperationRequireIdField();
			}

			var idValue = (int)idField
				.GetMapper<TSPListItem>()
				.MemberAccessor
				.GetValue(item);

			UpdateInternal(idValue, item, contentType);
		}

		public void Delete<T>(T item)
		{
			if (List.IsExternal)
			{
				throw Error.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw Error.OperationRequireIdField();
			}

			var idValue = (int)idField
				.GetMapper<TSPListItem>()
				.MemberAccessor
				.GetValue(item);

			DeleteInternal(idValue, contentType);
		}

		/// <summary>
		/// Converts query model to CAML-string in next format <![CDATA[<View><Query></Query></View>]]>.
		/// </summary>
		/// <typeparam name="T">Content Type.</typeparam>
		/// <param name="queryModel">Query model to convert.</param>
		/// <param name="filterByContentTypeId"></param>
		/// <returns>CAML-string in next format <![CDATA[<View><Query></Query></View>]]></returns>
		protected string ConvertToCamlString([NotNull]QueryModel queryModel, [NotNull]MetaContentType contentType, bool filterByContentTypeId = true)
		{
			if (filterByContentTypeId && !List.IsExternal)
			{
				queryModel.MergeWheres(new ComparisonModel(ComparisonOperator.Eq, new ContentTypeIdRefModel(), contentType.Id)
				{
					IsValueConverted = true
				});
			}

			return new CamlQueryTranslator(contentType).Process(queryModel);
		}

		protected void UpdateViewFields([NotNull] QueryModel query, [NotNull] MetaContentType contentType)
		{
			if (!query.SelectableFields.IsNullOrEmpty())
			{
				return;
			}

			var viewFields = ((IEnumerable<MetaField>)contentType.Fields)
				.Select(n => new MemberRefModel(n.Member))
				.ToList();

			query.MergeSelectableFields(viewFields);
		}

		protected abstract IList<TSPListItem> FetchInternal(string caml);

		protected abstract TSPListItem GetInternal(int id, MetaContentType contentType);

		protected abstract int AddInternal(object item, MetaContentType contentType);

		protected abstract void UpdateInternal(int id, object item, MetaContentType contentType);

		protected abstract void DeleteInternal(int id, MetaContentType contentType);

		protected T Materialize<T>(TSPListItem spItem, MetaContentType contentType, IReadOnlyCollection<MemberRefModel> fields = null)
		{
			var mapper = contentType.GetMapper<TSPListItem>();

			return (T)mapper.CreateAndMap(spItem, fields);
		}

		protected IEnumerable<T> Materialize<T>(IEnumerable<TSPListItem> spItems, MetaContentType contentType, IReadOnlyCollection<MemberRefModel> fields = null)
		{
			return spItems.Select(n => Materialize<T>(n, contentType, fields));
		}
	}
}