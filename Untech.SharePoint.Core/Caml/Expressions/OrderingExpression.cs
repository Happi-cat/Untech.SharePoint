using System;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class OrderingExpression : CamlExpression
	{
		public OrderingExpression(Type sourceType, Expression fieldRef, bool ascending)
		{
			FieldRef = fieldRef;
			Ascending = ascending;
		}

		public Expression FieldRef { get; private set; }

		public bool Ascending { get; private set; }

		public override ExpressionType NodeType { get { return (ExpressionType) CamlExpressionType.Ordering; } }

		public override Type Type { get { return FieldRef.Type; } }

		protected internal override Expression Accept(CamlExpressionVisitor visitor)
		{
			return visitor.VisitOrderingExpression(this);
		}

		public override string ToString()
		{
			return string.Format(".Order( {0}, Ascending: {1} )", FieldRef, Ascending);
		}
	}
}