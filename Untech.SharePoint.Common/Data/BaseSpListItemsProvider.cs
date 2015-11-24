using JetBrains.Annotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	public class BaseSpListItemsProvider
	{
		public BaseSpListItemsProvider([NotNull] MetaList list)
		{
			Guard.CheckNotNull("list", list);

			List = list;
		}

		[NotNull]
		public MetaList List { get; private set; }

		protected string ConvertToCamlString<T>([NotNull]QueryModel queryModel, bool contentTypeFilter = true)
		{
			var contentType = List.ContentTypes[typeof (T)];

			if (contentTypeFilter)
			{
				AppendContentTypeFilter(contentType, queryModel);
			}

			return new CamlQueryTranslator(contentType).Process(queryModel);
		}

		private void AppendContentTypeFilter([NotNull]MetaContentType contentType, [NotNull]QueryModel queryModel)
		{
			if (contentType.List.IsExternal)
			{
				return;
			}

			queryModel.MergeWheres(new ComparisonModel(ComparisonOperator.Eq, new ContentTypeIdRefModel(), contentType.Id));
		}
	}
}