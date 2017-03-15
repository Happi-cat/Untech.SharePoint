using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	[TestClass]
	public class XorRewriterTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void CanRewriteSimple()
		{
			Given(n => n.Bool1 ^ n.Bool2)
				.Expected(n => n.Bool1 && !n.Bool2 || !n.Bool1 && n.Bool2);
		}

		[TestMethod]
		public void CanRewriteWithCall()
		{
			Given(n => n.Bool1 ^ n.String1.Contains("A"))
				.Expected(n => n.Bool1 && !n.String1.Contains("A") || !n.Bool1 && n.String1.Contains("A"));
		}

		protected override ExpressionVisitor Visitor
		{
			get { return new XorRewriter(); }
		}
	}
}