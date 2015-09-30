using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.ExpressionVisitors;

namespace Untech.SharePoint.Common.Test.Data.Translators.ExpressionVisitors
{
	[TestClass]
	public class StringIsNullOrEmptyRewriterTest : BaseExpressionVisitorTest<StringIsNullOrEmptyRewriter>
	{
		[TestMethod]
		public void CanRewrite()
		{
			Test(n => string.IsNullOrEmpty(n.String1), n => n.String1 == null || n.String1 == "");
		}
	}
}
