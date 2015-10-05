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
			Test(n => 1 == n.Int1, n => n.Int1 == 1);
			Test(n => 10 < n.Int1, n => n.Int1 > 10);
			Test(n => n.Int1 < 100, n => n.Int1 < 100);
		}

		protected override ExpressionVisitor Visitor
		{
			get { return new SwapComparisonVisitor(); }
		}
	}
}
