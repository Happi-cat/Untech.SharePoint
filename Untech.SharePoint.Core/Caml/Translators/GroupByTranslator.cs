using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class GroupByTranslator : ExpressionVisitor, ICamlTranslator
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
			base.VisitMethodCall(node);

			if (node.Method.Name == "GroupBy")
			{
				AddFieldRef(VisitGroupBy(node));
			}
			return node;
		}

		private void AddFieldRef(XElement element)
		{
			if (element == null)
			{
				return;
			}

			if (Root == null)
			{
				Root = new XElement(Tags.GroupBy);
			}

			Root.Add(element);
		}

		private XElement VisitGroupBy(MethodCallExpression node)
		{
			var lambdaExpression = (LambdaExpression)node.Arguments[1].StripQuotes();

			return TranslatorHelpers.GetFieldRef(lambdaExpression.Body);
		}
	}
}