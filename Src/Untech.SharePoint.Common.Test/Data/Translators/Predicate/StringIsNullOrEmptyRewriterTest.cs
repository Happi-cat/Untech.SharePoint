using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.Predicate;

namespace Untech.SharePoint.Common.Test.Data.Translators.Predicate
{
	[TestClass]
	public class StringIsNullOrEmptyRewriterTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void CanRewrite()
		{
			Given(n => string.IsNullOrEmpty(n.String1)).Expected(n => n.String1 == null || n.String1 == "");
		}

		protected override ExpressionVisitor Visitor
		{
			get { return new StringIsNullOrEmptyRewriter(); }
		}
	}
}
