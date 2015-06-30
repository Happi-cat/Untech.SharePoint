using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class LimitTranslator : ExpressionVisitor, ICamlTranslator
	{
		private XElement _root;

		public XElement Translate(ISpModelContext modelContext, Expression predicate)
		{
			Visit(predicate);

			return _root;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			switch (node.Method.Name)
			{
				case "Take":
					_root = VisitLimit((ConstantExpression)node.Arguments[1].StripQuotes());
					break;
				case "First":
				case "FirstOrDefault":
				case "Single":
				case "SingleOrDefault":
					_root = VisitLimit(Expression.Constant(1));
					break;
				case "Skip":
					throw new NotSupportedException("Method 'Skip' not supported");
			}

			return base.VisitMethodCall(node);
		}

		private XElement VisitLimit(ConstantExpression node)
		{
			return new XElement(Tags.RowLimit, node.Value);
		}
	}
}