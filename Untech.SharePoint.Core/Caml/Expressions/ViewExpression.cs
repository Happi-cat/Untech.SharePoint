using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class ViewExpression : CamlExpression
	{
		private readonly Type _type;

		public ViewExpression(Type type, ReadOnlyCollection<Expression> viewFields, Expression query, Expression rowlimit)
		{
			_type = type;
			ViewFields = viewFields;
			Query = query;
			RowLimit = rowlimit;
		}

		public ReadOnlyCollection<Expression> ViewFields { get; private set; }

		public Expression Query { get; private set; }

		public Expression RowLimit { get;private set; }

		public override ExpressionType NodeType { get { return (ExpressionType) CamlExpressionType.View; } }

		public override Type Type { get { return _type; } }

		public ViewExpression Update(ReadOnlyCollection<Expression> viewFields, Expression query, Expression rowlimit)
		{
			if (ViewFields == viewFields && Query == query && RowLimit == rowlimit)
			{
				return this;
			}

			return new ViewExpression(_type, viewFields, query, rowlimit);
		}

		protected internal override Expression Accept(CamlExpressionVisitor visitor)
		{
			return visitor.VisitViewExpression(this);
		}

		public override string ToString()
		{
			return string.Format(".View( {0}, {1}, {2} )", ViewFields, Query, RowLimit);
		}
	}
}