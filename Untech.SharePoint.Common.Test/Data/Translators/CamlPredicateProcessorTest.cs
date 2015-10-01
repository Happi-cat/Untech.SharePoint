using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	[TestClass]
	public class CamlPredicateProcessorTest
	{
		[TestMethod]
		public void CanConvert()
		{
			Test(n => n.String1.Contains("1") && n.Int1 == 2, "");
		}

		public void Test(Expression<Func<VisitorsTestClass, bool>> original, string exprected)
		{
			var processor = new CamlPredicateProcessor();

			Assert.AreEqual(exprected, processor.Process(original).ToString());
		}
	}
}