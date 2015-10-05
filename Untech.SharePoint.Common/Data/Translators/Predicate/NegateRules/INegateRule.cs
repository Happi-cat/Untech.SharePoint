using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate.NegateRules
{
	internal interface INegateRule
	{
		bool CanNegate(Expression node);

		Expression Negate(Expression node);
	}
}