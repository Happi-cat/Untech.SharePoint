using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class QueryExpression : CamlExpression
	{
		private readonly Type _type;

		public QueryExpression(Type type, Expression where,
			ReadOnlyCollection<Expression> orderBy,
			ReadOnlyCollection<Expression> groupBy)
		{
			_type = type;
			Where = where;
			OrderBy = orderBy;
			GroupBy = groupBy;
		}

		public Expression Where { get;  private set; }

		public ReadOnlyCollection<Expression> OrderBy { get; private set; }

		public ReadOnlyCollection<Expression> GroupBy { get; private set; }

		public override ExpressionType NodeType { get { return (ExpressionType) CamlExpressionType.Query; } }

		public override Type Type { get { return _type; } }

		public QueryExpression Update(Expression where,
			ReadOnlyCollection<Expression> orderBy,
			ReadOnlyCollection<Expression> groupBy)
		{
			if (where == Where && orderBy == OrderBy && groupBy == GroupBy)
			{
				return this;
			}

			return new QueryExpression(_type, where, orderBy, groupBy);
		}

		protected internal override Expression Accept(CamlExpressionVisitor visitor)
		{
			return visitor.VisitQueryExpression(this);
		}

		public override string ToString()
		{
			return string.Format(".Query( Where: {0}, OrderBy: {1}, GroupBy: {2} )", Where, OrderBy, GroupBy);
		}
	}
}