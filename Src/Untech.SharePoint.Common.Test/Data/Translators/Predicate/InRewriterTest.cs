using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Extensions;

namespace Untech.SharePoint.Data.Translators.Predicate
{
	[TestClass]
	public class InRewriterTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void Visit_Rewrites_WhenInArray()
		{
			var possibleValues = new[] { "#1", "#2", "#3" };

			Given(n => n.String1.In(possibleValues))
				.PreEvaluate()
				.Expected(n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenInList()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };

			Given(n => n.String1.In(possibleValues))
				.PreEvaluate()
				.Expected(n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenNotInArray()
		{
			var possibleValues = new[] { "#1", "#2", "#3" };

			Given(n => !n.String1.In(possibleValues))
				.PreEvaluate()
				.Expected(n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenNotInList()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };

			Given(n => !n.String1.In(possibleValues))
				.PreEvaluate()
				.Expected(n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenArrayContains()
		{
			var possibleValues = new[] { "#1", "#2", "#3" };

			Given(n => possibleValues.Contains(n.String1))
				.PreEvaluate()
				.Expected(n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenListContains()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };

			Given(n => possibleValues.Contains(n.String1))
				.PreEvaluate()
				.Expected(n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenEnumerableContains()
		{
			IEnumerable<string> possibleValues = new List<string> { "#1", "#2", "#3" };

			Given(n => possibleValues.Contains(n.String1))
				.PreEvaluate()
				.Expected(n => n.String1 == "#1" || n.String1 == "#2" || n.String1 == "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenArrayNotContains()
		{
			var possibleValues = new[] { "#1", "#2", "#3" };

			Given(n => !possibleValues.Contains(n.String1))
				.PreEvaluate()
				.Expected(n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenListNotContains()
		{
			var possibleValues = new List<string> { "#1", "#2", "#3" };

			Given(n => !possibleValues.Contains(n.String1))
				.PreEvaluate()
				.Expected(n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenEnumerableNotContains()
		{
			IEnumerable<string> possibleValues = new List<string> { "#1", "#2", "#3" };

			Given(n => !possibleValues.Contains(n.String1))
				.PreEvaluate()
				.Expected(n => n.String1 != "#1" && n.String1 != "#2" && n.String1 != "#3");
		}

		[TestMethod]
		public void Visit_Rewrites_WhenStringContains()
		{
			Given(n => n.String1.Contains("TEST"))
				.PreEvaluate()
				.Expected(n => n.String1.Contains("TEST"));
		}


		[TestMethod]
		public void Visit_NotRewrites_WhenArrayPropertyContains()
		{
			Given(n => n.StringCollection1.Contains("TEST"))
				.PreEvaluate()
				.Expected(n => n.StringCollection1.Contains("TEST"));
		}

		[TestMethod]
		public void Visit_NotRewrites_WhenEnumerablePropertyContains()
		{
			Given(n => n.StringCollection2.Contains("TEST"))
				.PreEvaluate()
				.Expected(n => n.StringCollection2.Contains("TEST"));
		}

		[TestMethod]
		public void Visit_NotRewrites_WhenCollectionPropertyContains()
		{
			Given(n => n.StringCollection3.Contains("TEST"))
				.PreEvaluate()
				.Expected(n => n.StringCollection3.Contains("TEST"));
		}

		[TestMethod]
		public void Visit_NotRewrites_WhenListPropertyContains()
		{
			Given(n => n.StringCollection4.Contains("TEST"))
				.PreEvaluate()
				.Expected(n => n.StringCollection4.Contains("TEST"));
		}

		protected override ExpressionVisitor TestableVisitor => new InRewriter();
	}
}
