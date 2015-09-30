using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.ExpressionVisitors;

namespace Untech.SharePoint.Common.Test.Data.Translators.ExpressionVisitors
{
	[TestClass]
	public class RedundantConditionRemoverTest :BaseExpressionVisitorTest<RedundantConditionRemover>
	{
		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
		public void CanRemove()
		{
			Test(n => n.Bool1 || (n.Bool2 && true), n => n.Bool1 || n.Bool2);
			Test(n => n.Bool1 || (n.Bool2 && false), n => n.Bool1);

			Test(n => n.Bool1 && (n.Bool2 || true), n => n.Bool1);
			Test(n => n.Bool1 && (n.Bool2 || false), n => n.Bool1 && n.Bool2);
		}
	}
}
