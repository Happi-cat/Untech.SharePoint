using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Caml.Finders;
using Untech.SharePoint.Core.Caml.Modifiers;
using Untech.SharePoint.Core.Caml.Translators;

namespace Untech.SharePoint.Core.Caml
{
	internal class CamlTranslator : ICamlTranslator
	{
		protected IReadOnlyCollection<ExpressionVisitor> TreeVisitors { get; private set; }

		protected IReadOnlyCollection<ICamlTranslator> Translators { get; private set; }

		protected SpModelContextFinder ModelContextFinder { get; private set; }

		protected ProjectorFinder ProjectorFinder { get; private set; }

		public CamlTranslator()
		{
			ModelContextFinder = new SpModelContextFinder();

			ProjectorFinder = new ProjectorFinder();

			TreeVisitors = new List<ExpressionVisitor>
			{
				new WhereModifier(),
				new PredicateOptimizer(),
				new Evaluator()
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

		protected internal XElement Translate(Expression node)
		{
			ModelContextFinder.Visit(node);
			

			return Translate(ModelContextFinder.ModelContext, node);
		}

		public XElement Translate(ISpModelContext modelContext, Expression node)
		{
			node = TreeVisitors.Aggregate(node, (current, treeVisitor) => treeVisitor.Visit(current));

			var queryElement = new XElement(Tags.Query, 
				Translators.Select(translator => translator.Translate(modelContext, node)));

			return queryElement;
		}
	}
}