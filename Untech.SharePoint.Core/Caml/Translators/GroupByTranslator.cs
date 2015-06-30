using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class GroupByTranslator : ExpressionVisitor, ICamlTranslator
	{
		private XElement _root;

		public XElement Translate(ISpModelContext modelContext, Expression predicate)
		{
			Visit(predicate);

			return _root;
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

			XElement currentElement = null;
			switch (node.Method.Name)
			{
				case "GroupBy":
					currentElement = VisitGroupBy(node);
					break;
			}
			AddFieldRef(currentElement);
			return node;
		}

		private void AddFieldRef(XElement element)
		{
			if (element == null)
			{
				return;
			}

			if (_root == null)
			{
				_root = new XElement(Tags.GroupBy);
			}

			_root.Add(element);
		}

		private XElement VisitGroupBy(MethodCallExpression node)
		{
			var lambdaExpression = (LambdaExpression)node.Arguments[1].StripQuotes();
			var memberExpression = (MemberExpression)lambdaExpression.Body.StripQuotes();

			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Name, memberExpression.Member.Name));
		}
	}
}