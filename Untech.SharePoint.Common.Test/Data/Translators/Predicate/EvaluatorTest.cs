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

		protected override ExpressionVisitor Visitor
		{
			get { return new Evaluator(); }
		}
	}
}
