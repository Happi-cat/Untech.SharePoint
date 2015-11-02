using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.Predicate;

namespace Untech.SharePoint.Common.Test.Data.Translators.Predicate
{
	[TestClass]
	public class SwapComparisonVisitorTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void CanSwap()
		{
			Given(n => 1 == n.Int1).Expected(n => n.Int1 == 1);
			Given(n => 10 < n.Int1).Expected(n => n.Int1 > 10);
			Given(n => n.Int1 < 100).Expected(n => n.Int1 < 100);
		}

		protected override ExpressionVisitor Visitor
		{
			get { return new SwapComparisonVisitor(); }
		}
	}
}
