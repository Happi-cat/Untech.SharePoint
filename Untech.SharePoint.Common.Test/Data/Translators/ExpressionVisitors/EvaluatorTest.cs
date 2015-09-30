using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.ExpressionVisitors;

namespace Untech.SharePoint.Common.Test.Data.Translators.ExpressionVisitors
{
	[TestClass]
	public class EvaluatorTest : BaseExpressionVisitorTest<Evaluator>
	{
		[TestMethod]
		public void CanEvaluateCall()
		{
			Test(n => n.String1 == GetSomeExternalString(), n=> n.String1 == "TEST");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantBoolCompare")]
		public void CanEvaluateCondition()
		{
			var a = true;
			var b = false;
			Test(n => n.Bool1 == (a || b), n => n.Bool1 == true);
		}

		private string GetSomeExternalString()
		{
			return "TEST";
		}
	}
}
