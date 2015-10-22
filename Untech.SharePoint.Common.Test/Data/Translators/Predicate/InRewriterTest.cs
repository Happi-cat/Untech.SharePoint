using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.Predicate;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Test.Data.Translators.Predicate
{
	[TestClass]
	public class InRewriterTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void CanRewriteInArray()
		{
			var possibleValues = new[] {"#1", "#2", "#3"};
			TestWitEvaluator(n => n.String1.In(possibleValues), n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void CanRewriteInList()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };
			TestWitEvaluator(n => n.String1.In(possibleValues), n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void CanRewriteNotInArray()
		{
			var possibleValues = new[] { "#1", "#2", "#3" };
			TestWitEvaluator(n => !n.String1.In(possibleValues), n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void CanRewriteNotInList()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };
			TestWitEvaluator(n => !n.String1.In(possibleValues), n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void CanRewriteArrayContains()
		{
			var possibleValues = new[] { "#1", "#2", "#3" };
			TestWitEvaluator(n => possibleValues.Contains(n.String1), n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void CanRewriteListContains()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };
			TestWitEvaluator(n => possibleValues.Contains(n.String1), n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void CanRewriteEnumerableContains()
		{
			IEnumerable<string> possibleValues = new List<string> { "#1", "#2", "#3" };
			TestWitEvaluator(n => possibleValues.Contains(n.String1), n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void CanRewriteArrayNotContains()
		{
			var possibleValues = new[] { "#1", "#2", "#3" };
			TestWitEvaluator(n => !possibleValues.Contains(n.String1), n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void CanRewriteListNotContains()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };
			TestWitEvaluator(n => !possibleValues.Contains(n.String1), n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void CanRewriteEnumerableNotContains()
		{
			IEnumerable<string> possibleValues = new List<string> { "#1", "#2", "#3" };
			TestWitEvaluator(n => !possibleValues.Contains(n.String1), n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void CannotRewriteStringContains()
		{
			TestWitEvaluator(n => n.String1.Contains("TEST"), n => n.String1.Contains("TEST"));
		}

		protected override ExpressionVisitor Visitor
		{
			get { return new InRewriter(); }
		}
	}
}
