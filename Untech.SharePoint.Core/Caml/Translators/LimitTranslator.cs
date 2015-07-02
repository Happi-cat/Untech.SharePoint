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

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			Visit(node.Arguments[0]);

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
				case "TakeWhile":
				case "Skip":
				case "SkipWhile":
					throw new NotSupportedException(string.Format("Method '{0}' not supported", node.Method.Name));
			}

			return node;
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