using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal static class CamlExpressionExtensions
	{
		internal static bool IsCamlExpression(this ExpressionType nodeType)
		{
			return (int) nodeType >= 1000;
		}
	}
}