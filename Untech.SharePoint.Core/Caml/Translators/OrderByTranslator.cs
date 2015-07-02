using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class OrderByTranslator : ExpressionVisitor, ICamlTranslator
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

			switch (node.Method.Name)
			{
				case "OrderBy":
				case "ThenBy":
					AddFieldRef(VisitOrderBy(node, true));
					break;
				case "OrderByDescending":
				case "ThenByDescending":
					AddFieldRef(VisitOrderBy(node, false));
					break;
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
				Root = new XElement(Tags.OrderBy);
			}

			Root.Add(element);
		}

		private XElement VisitOrderBy(MethodCallExpression node, bool ascending)
		{
			var lambdaExpression = (LambdaExpression) node.Arguments[1].StripQuotes();

			var ascendingAttribute = new XAttribute(Tags.Ascending, ascending.ToString().ToUpperInvariant());

			var fieldRefElement = TranslatorHelpers.GetFieldRef(ModelContext, lambdaExpression.Body);
			fieldRefElement.Add(ascendingAttribute);
			return fieldRefElement;
		}
	}
}