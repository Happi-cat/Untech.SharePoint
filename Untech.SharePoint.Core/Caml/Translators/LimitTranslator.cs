using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class LimitTranslator : ExpressionVisitor, ICamlTranslator
	{
		protected XElement Root { get; private set; }

		public XElement Translate(ISpModelContext modelContext, Expression predicate)
		{
			Visit(predicate);

			return Root;
		}

		public override Expression Visit(Expression node)
		{
			if (node == null || node.NodeType != ExpressionType.Call)
			{
				return node;
			}
			return base.Visit(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			switch (node.Method.Name)
			{
				case "Take":
					Root = VisitLimit(node.Arguments[1]);
					break;
				case "First":
				case "FirstOrDefault":
				case "Single":
				case "SingleOrDefault":
					Root = VisitLimit(Expression.Constant(1));
					break;
				case "Skip":
					throw new NotSupportedException("Method 'Skip' not supported");
			}

			return base.VisitMethodCall(node);
		}

		private XElement VisitLimit(Expression node)
		{
			node = node.StripQuotes();

			if (node.NodeType != ExpressionType.Constant)
			{
				throw new NotSupportedException(string.Format("Limit {0} not supported", node));
			}

			var constExpression = (ConstantExpression) node;

			return new XElement(Tags.RowLimit, constExpression.Value);
		}
	}
}