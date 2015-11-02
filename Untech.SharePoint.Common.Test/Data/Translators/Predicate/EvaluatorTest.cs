using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.Predicate;

namespace Untech.SharePoint.Common.Test.Data.Translators.Predicate
{
	[TestClass]
	public class EvaluatorTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void CanEvaluateCall()
		{
			Given(n => n.String1 == GetSomeExternalString())
				.Expected(n => n.String1 == "TEST");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantBoolCompare")]
		public void CanEvaluateCondition()
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

		protected override ExpressionVisitor Visitor
		{
			get { return new Evaluator(); }
		}
	}
}
