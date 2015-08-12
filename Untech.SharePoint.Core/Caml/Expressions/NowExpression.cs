using System;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class NowExpression : CamlExpression
	{
		public override ExpressionType NodeType { get { return (ExpressionType)CamlExpressionType.Now; } }

		public override Type Type { get { return typeof(DateTime); } }

		protected internal override Expression Accept(CamlExpressionVisitor visitor)
		{
			return visitor.VisitNowExpression(this);
		}

		public override string ToString()
		{
			return ".Now";
		}
	}
}