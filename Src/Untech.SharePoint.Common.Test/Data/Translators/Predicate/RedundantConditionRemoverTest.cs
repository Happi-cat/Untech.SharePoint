using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Data.Translators.Predicate
{
	[TestClass]
	public class RedundantConditionRemoverTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
		public void Visit_Optimizes()
		{
			Given(n => n.Bool1 || (n.Bool2 && true)).Expected(n => n.Bool1 || n.Bool2);
			Given(n => n.Bool1 || (n.Bool2 && false)).Expected(n => n.Bool1);

			Given(n => n.Bool1 && (n.Bool2 || true)).Expected(n => n.Bool1);
			Given(n => n.Bool1 && (n.Bool2 || false)).Expected(n => n.Bool1 && n.Bool2);
		}

		protected override ExpressionVisitor TestableVisitor => new RedundantConditionRemover();
	}
}
