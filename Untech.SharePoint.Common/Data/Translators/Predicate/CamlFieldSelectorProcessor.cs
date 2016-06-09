using System.Linq.Expressions;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class CamlFieldSelectorProcessor : IProcessor<Expression, MemberRefModel>
	{
		[NotNull]
		public MemberRefModel Process([NotNull] Expression predicate)
		{
			Guard.CheckNotNull(nameof(predicate), predicate);

			Logger.Trace(LogCategories.FieldSelectorProcessor, "Original predicate:\n{0}", predicate);

			predicate = predicate.StripQuotes();
			if (predicate.NodeType == ExpressionType.Lambda)
			{
				predicate = ((LambdaExpression)predicate).Body;
			}

			var result = CamlProcessorUtils.GetFieldRef(predicate);

			Logger.Trace(LogCategories.FieldSelectorProcessor, "Selectable field in predicate:\n{0}", result);

			return result;
		}
	}
}