using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Core.Caml.Modifiers;
using System.Collections.Generic;

namespace Untech.SharePoint.Core.Test.Caml.Modifiers
{
	[TestClass]
	public class PredicateModifierTest
	{
		[TestMethod]
		public void CanModifyWherePredicate()
		{
			var query = (new List<SimpleModel>()).AsQueryable();

			var visitor = new PredicateModifier();

			var original = query.Where(n => n.Bool1);
			var expected = query.Where(n => n.Bool1 == true);

			var actual = visitor.Visit(original.Expression);

			Assert.AreEqual(expected.Expression, actual);
		}

		[TestMethod]
		public void CanModifyAndOrPredicate()
		{
			var query = (new List<SimpleModel>()).AsQueryable();

			var visitor = new PredicateModifier();

			var original = query.Where(n => n.String1 == "TEST" && n.Bool1);
			var expected = query.Where(n => n.String1 == "TEST" && n.Bool1 == true);

			var actual = visitor.Visit(original.Expression);

			Assert.AreEqual(expected.Expression, actual);
		}
	}
}
