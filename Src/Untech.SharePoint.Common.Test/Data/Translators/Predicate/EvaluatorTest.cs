using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	[TestClass]
	public class EvaluatorTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void Visit_EvaluateCall()
		{
			Given(n => n.String1 == GetSomeExternalString())
				.Expected(n => n.String1 == "TEST");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantBoolCompare")]
		public void Visit_EvaluateCondition()
		{
			var a = true;
			var b = false;

			Given(n => n.Bool1 == (a || b))
				.Expected(n => n.Bool1 == true);
		}

		private string GetSomeExternalString()
		{
			return "TEST";
		}

		protected override ExpressionVisitor TestableVisitor => new Evaluator();
	}
}
