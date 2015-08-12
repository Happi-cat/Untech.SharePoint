using System;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class CamlExpression : Expression
	{
		protected override Expression Accept(ExpressionVisitor visitor)
		{
			var camlVisitor = visitor as CamlExpressionVisitor;
			return camlVisitor != null ? Accept(camlVisitor) : base.Accept(visitor);
		}

		protected internal virtual Expression Accept(CamlExpressionVisitor visitor)
		{
			throw new NotImplementedException();
		}
	}
}