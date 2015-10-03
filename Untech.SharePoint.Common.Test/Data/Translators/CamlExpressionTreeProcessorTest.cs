using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	[TestClass]
	public class CamlExpressionTreeProcessorTest
	{
		[TestMethod]
		public void CanBeRun()
		{
			TestModel(source => source, "<Query><RowLimit>0</RowLimit></Query>");
		}

		[TestMethod]
		public void CanProcessSingleWhere()
		{
			TestModel(source => source.Where(n => n.Bool1 && n.Int1 > 10 || n.String1.StartsWith("TEST")),
				"<Query><RowLimit>0</RowLimit><Where><Or><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And><BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></Or></Where></Query>");
		}

		[TestMethod]
		public void CanProcessMultipleWhere()
		{
			TestModel(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Where(n => n.String1.StartsWith("TEST")), 
				"<Query><RowLimit>0</RowLimit>"+
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>"+
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>"+
				"</Query>");
		}

		protected void TestModel(Func<IQueryable<VisitorsTestClass>, IQueryable<VisitorsTestClass>> queryBuilder, string expected)
		{
			var query = queryBuilder(new FakeQueryable<VisitorsTestClass>());

			var generatedExpression = new CamlExpressionTreeProcessor().Visit(query.Expression);

			if (generatedExpression.NodeType != ExpressionType.Call)
			{
				Assert.Fail("Generated expression is not a method call");
			}

			var callNode = (MethodCallExpression) generatedExpression;
			if (!OpUtils.IsOperator(callNode.Method, OpUtils.SpqGetItems))
			{
				Assert.Fail("Not a SpQueryable.GetSpItems");
			}

			var model = (QueryModel)((ConstantExpression) callNode.Arguments[1].StripQuotes()).Value;
			Assert.AreEqual(model.ToString(), expected);
		}
	}
}
