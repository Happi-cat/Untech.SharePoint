using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class GroupByTranslator : ExpressionVisitor, ICamlTranslator
	{
		protected XElement Root { get; private set; }
		protected ISpModelContext ModelContext { get; private set; }

		public XElement Translate(ISpModelContext modelContext, Expression predicate)
		{
			ModelContext = modelContext;

			Visit(predicate);

			return Root;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			Visit(node.Arguments[0]);

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

			return TranslatorHelpers.GetFieldRef(ModelContext, lambdaExpression.Body);
		}
	}
}