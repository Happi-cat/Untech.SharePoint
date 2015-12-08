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
	public abstract class BaseSpListItemsProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSpListItemsProvider" />
		/// </summary>
		/// <param name="list">Meta models of the SP list.</param>
		protected BaseSpListItemsProvider([NotNull] MetaList list)
		{
			Guard.CheckNotNull("list", list);

			List = list;
		}

		/// <summary>
		/// Gets list associated with this instance of the <see cref="BaseSpListItemsProvider"/>.
		/// </summary>
		[NotNull]
		public MetaList List { get; private set; }

		/// <summary>
		/// Converts query model to CAML-string in next format <![CDATA[<View><Query></Query></View>]]>.
		/// </summary>
		/// <typeparam name="T">Content Type.</typeparam>
		/// <param name="queryModel">Query model to convert.</param>
		/// <param name="filterByContentTypeId"></param>
		/// <returns>CAML-string in next format <![CDATA[<View><Query></Query></View>]]></returns>
		protected string ConvertToCamlString<T>([NotNull]QueryModel queryModel, bool filterByContentTypeId = true)
		{
			var contentType = List.ContentTypes[typeof (T)];

			if (filterByContentTypeId)
			{
				AppendContentTypeFilter(contentType, queryModel);
			}

			return new CamlQueryTranslator(contentType).Process(queryModel);
		}

		[NotNull]
		protected List<MemberRefModel> GetAndRewriteViewFields<T>([NotNull]QueryModel caml)
		{
			var viewFields = caml.SelectableFields.EmptyIfNull().ToList();
			if (viewFields.Count == 0)
			{
				var contentType = List.ContentTypes[typeof(T)];
				viewFields = ((IEnumerable<MetaField>)contentType.Fields).Select(n => new MemberRefModel(n.Member)).ToList();
				caml.MergeSelectableFields(viewFields);
			}
			return viewFields;
		}

		private static void AppendContentTypeFilter([NotNull]MetaContentType contentType, [NotNull]QueryModel queryModel)
		{
			if (contentType.List.IsExternal)
			{
				return;
			}

			queryModel.MergeWheres(new ComparisonModel(ComparisonOperator.Eq, new ContentTypeIdRefModel(), contentType.Id)
			{
				IsValueConverted = true
			});
		}
	}
}