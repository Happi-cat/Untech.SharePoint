using System;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class TodayExpression : CamlExpression
	{
		public TodayExpression()
		{
			Offset = Empty();
		}

		public TodayExpression(Expression offset)
		{
			Offset = offset;
		}

		public Expression Offset { get; private set; }

		public override ExpressionType NodeType { get { return (ExpressionType) CamlExpressionType.Today; } }

		public override Type Type { get { return typeof (DateTime); } }

		public TodayExpression Update(Expression offset)
		{
			return offset == Offset ? this: new TodayExpression(offset);
		}

		protected internal override Expression Accept(CamlExpressionVisitor visitor)
		{
			return visitor.VisitTodayExpression(this);
		}

		public override string ToString()
		{
			return string.Format(".Today( Offset: '{0}' )", Offset);
		}
	}
}