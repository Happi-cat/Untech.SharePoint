using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Caml.Modifiers;
using Untech.SharePoint.Core.Caml.Translators;

namespace Untech.SharePoint.Core.Caml
{
	internal class CamlTranslator
	{
		protected IReadOnlyCollection<ExpressionVisitor> TreeModifiers { get; private set; }

		protected IReadOnlyCollection<ICamlTranslator> Translators { get; private set; }

		public CamlTranslator()
		{
			TreeModifiers = new List<ExpressionVisitor>
			{
				new WhereModifier(),
				new PredicateModifier(),
				new Evaluator(), 
			};

			Translators = new List<ICamlTranslator>
			{
				new ViewFieldsTranslator(),
				new WhereTranslator(),
				new OrderByTranslator(),
				new GroupByTranslator(),
				new LimitTranslator()
			};
		}

		public string Translate(ISpModelContext modelContext, Expression node)
		{
			node = TreeModifiers.Aggregate(node, (current, treeVisitor) => treeVisitor.Visit(current));

			var queryElement = new XElement(Tags.Query, 
				Translators.Select(translator => translator.Translate(modelContext, node)));

			return queryElement.ToString();
		}
	}
}