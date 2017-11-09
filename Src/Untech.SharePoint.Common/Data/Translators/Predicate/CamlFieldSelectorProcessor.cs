using System.Linq.Expressions;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data.QueryModels;
using Untech.SharePoint.Diagnostics;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Data.Translators.Predicate
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