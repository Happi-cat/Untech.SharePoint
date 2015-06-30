using System;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class Evaluator : ExpressionVisitor
	{
		protected Nominator Nominator { get; private set; }

		public Evaluator()
		{
			Nominator = new Nominator();
		}

		public Evaluator(Nominator nominator)
		{
			if (nominator == null) 
				throw new ArgumentNullException("nominator");

			Nominator = nominator;
		}

		public override Expression Visit(Expression node)
		{
			Nominator.Reset();
			Nominator.Visit(node);
			return new SubtreeEvaluator(Nominator.Candidates).Visit(node);
		}
	}
}