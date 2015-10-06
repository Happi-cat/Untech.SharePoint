using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class CamlKeySelectorProcessor
	{
		public FieldRefModel Process(Expression predicate)
		{
			predicate = predicate.StripQuotes();
			if (predicate.NodeType == ExpressionType.Lambda)
			{
				predicate = ((LambdaExpression)predicate).Body;
			}

			return CamlProcessorUtils.GetFieldRef(predicate);
		}
	}
}