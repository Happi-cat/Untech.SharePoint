using System;
using System.Diagnostics.CodeAnalysis;
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
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToAny")]
	public class CamlQueryTreeProcessorTest
	{
		[TestMethod]
		public void CanBeRun()
		{
			TestModel(source => source, "<Query></Query>");
		}

		[TestMethod]
		public void CanProcessSingleWhere()
		{
			TestModel(source => source.Where(n => n.Bool1 && n.Int1 > 10 || n.String1.StartsWith("TEST")),
				"<Query>" +
				"<Where><Or><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></Or></Where>" +
				"</Query>");
		}

		[TestMethod]
		public void CanProcessMultipleWhere()
		{
			TestModel(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Where(n => n.String1.StartsWith("TEST")),
				"<Query>" +
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				"</Query>");
		}

		[TestMethod]
		public void CanProcessOrderBy()
		{
			TestModel(source => source.OrderBy(n => n.Bool1).ThenBy(n => n.String1),
				"<Query>" +
				"<OrderBy><FieldRef Name='Bool1' Ascending='TRUE' /><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
				"</Query>");
		}

		[TestMethod]
		public void CanProcessTake()
		{
			TestModel(
				source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Where(n => n.String1.StartsWith("TEST")).Take(10),
				"<Query><RowLimit>10</RowLimit>" +
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				"</Query>");
		}

		[TestMethod]
		public void CanProcessTakeAndOuterCalls()
		{
			TestModel(
				source =>
					source.Where(n => n.Bool1)
						.Where(n => n.Int1 > 10)
						.Where(n => n.String1.StartsWith("TEST"))
						.Take(10)
						.Where(n => n.String1 == "SOME"),
				"<Query><RowLimit>10</RowLimit>" +
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				"</Query>",
				source => source.Where(n => n.String1 == "SOME"));
		}

		[TestMethod]
		public void CanProcessSkip()
		{
			TestModel(
				source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Where(n => n.String1.StartsWith("TEST")).Skip(10),
				"<Query>" +
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				"</Query>");
		}

		[TestMethod]
		public void CanProcessSkipAndOuterCalls()
		{
			TestModel(
				source =>
					source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Where(n => n.String1.StartsWith("TEST")).Skip(10).Take(1),
				"<Query>" +
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				"</Query>",
				source => source.Take(1));
		}

		[TestMethod]
		public void CanProcessAny()
		{
			TestModel(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Any(),
				"<Query>" +
				"<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
				"</Query>");
		}


		[TestMethod]
		public void CanProcessAnyPredicate()
		{
			TestModel(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Any(n => n.String1.StartsWith("TEST")),
				"<Query>" +
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				"</Query>");
		}

		[TestMethod]
		public void CanProcessAll()
		{
			TestModel(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).All(n => n.String1 == "TEST"),
				"<Query>" +
				"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				"<Neq><FieldRef Name='String1' /><Value>TEST</Value></Neq></And></Where>" +
				"</Query>");

			TestModel(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).All(n => n.String1 == "TEST"),
				source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Any(n => n.String1 != "TEST"));
		}

		[TestMethod]
		public void ThrowForInvalidAll()
		{
			CustomAssert.Throw<NotSupportedException>(() =>
			{
				// NOTE: unable to process '!n.String1.StartsWith("TEST")'
				TestModel(
					source =>
						source.Where(n => n.Bool1)
							.Where(n => n.Int1 > 10)
							.All(n => n.String1.StartsWith("TEST")), "");
			});
		}

		[TestMethod]
		public void CanProcessFirst()
		{
			TestModel(source => source.OrderBy(n => n.String1).First(n => n.String2 == "TEST"),
				"<Query>" +
				"<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
				"<OrderBy><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
				"</Query>");

		}

		[TestMethod]
		public void CanProcessSingle()
		{
			TestModel(source => source.OrderBy(n => n.String1).Single(n => n.String2 == "TEST"),
				"<Query>" +
				"<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
				"<OrderBy><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
				"</Query>");

		}

		[TestMethod]
		public void CanProcessLast()
		{
			TestModel(source => source.Last(n => n.String2 == "TEST"),
				"<Query>" +
				"<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
				"<OrderBy><FieldRef Name='ID' Ascending='FALSE' /></OrderBy>" +
				"</Query>");

		}

		[TestMethod]
		public void CanProcessLastWithOrderBy()
		{
			TestModel(source => source.OrderBy(n => n.String1).Last(n => n.String2 == "TEST"),
				"<Query>" +
				"<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
				"<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
				"</Query>");

		}

		[TestMethod]
		public void CanProcessReverse()
		{
			TestModel(source => source.OrderBy(n => n.String1).Reverse(),
				"<Query>" +
				"<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
				"</Query>");

		}

		[TestMethod]
		public void CanProcessReverseAndOuterCalls()
		{
			TestModel(source => source.OrderBy(n => n.String1).ThenByDescending(n => n.String2).Reverse().Where(n => n.String2 == "TEST"),
				"<Query>" +
				"<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
				"<OrderBy><FieldRef Name='String1' Ascending='FALSE' /><FieldRef Name='String2' Ascending='TRUE' /></OrderBy>" +
				"</Query>");

		}

		protected void TestModel(Func<IQueryable<VisitorsTestClass>, IQueryable<VisitorsTestClass>> actualQueryBuilder,
			string expectedCaml, Func<IQueryable<VisitorsTestClass>, IQueryable<VisitorsTestClass>> expectedQueryBuilder = null)
		{
			var actualTree = actualQueryBuilder(new FakeQueryable<VisitorsTestClass>()).Expression;
			actualTree = new CamlQueryTreeProcessor().Process(actualTree);

			var queryModel = QueryFinder.FindQuery(actualTree) ?? new QueryModel();
			Assert.AreEqual(expectedCaml, queryModel.ToString());

			if (expectedQueryBuilder == null)
			{
				return;
			}

			var expectedTree = expectedQueryBuilder(new FakeQueryable<VisitorsTestClass>()).Expression;

			actualTree = new SpQueryRemover().Visit(actualTree);
			expectedTree = new SpQueryRemover().Visit(expectedTree);

			Assert.AreEqual(expectedTree.ToString(), actualTree.ToString());
		}

		protected void TestModel(Func<IQueryable<VisitorsTestClass>, object> actualQueryBuilder, string expectedCaml,
			Func<IQueryable<VisitorsTestClass>, object> expectedQueryBuilder = null)
		{
			Expression actualTree = null;
			actualQueryBuilder(new FakeQueryable<VisitorsTestClass>
			{
				ExpressionExecutor = node => { actualTree = node; }
			});
			actualTree = new CamlQueryTreeProcessor().Process(actualTree);

			var queryModel = QueryFinder.FindQuery(actualTree) ?? new QueryModel();
			Assert.AreEqual(expectedCaml, queryModel.ToString());

			if (expectedQueryBuilder == null)
			{
				return;
			}

			Expression expectedTree = null;
			expectedQueryBuilder(new FakeQueryable<VisitorsTestClass>
			{
				ExpressionExecutor = node => { expectedTree = node; }
			});

			actualTree = new SpQueryRemover().Visit(actualTree);
			expectedTree = new SpQueryRemover().Visit(expectedTree);

			Assert.AreEqual(expectedTree.ToString(), actualTree.ToString());
		}

		protected void TestModel(Func<IQueryable<VisitorsTestClass>, object> actualQueryBuilder,
			Func<IQueryable<VisitorsTestClass>, object> expectedTreeBuilder)
		{
			Expression actualTree = null;
			actualQueryBuilder(new FakeQueryable<VisitorsTestClass>
			{
				ExpressionExecutor = node => { actualTree = node; }
			});
			actualTree = new CamlQueryTreeProcessor().Process(actualTree);

			Expression expectedTree = null;
			expectedTreeBuilder(new FakeQueryable<VisitorsTestClass>
			{
				ExpressionExecutor = node => { expectedTree = node; }
			});
			expectedTree = new CamlQueryTreeProcessor().Process(expectedTree);

			Assert.AreEqual(expectedTree.ToString(), actualTree.ToString());
		}


		protected class QueryFinder : ExpressionVisitor
		{
			public static QueryModel FindQuery(Expression node)
			{
				var finder = new QueryFinder();

				finder.Visit(node);

				return finder.Query;
			}

			public QueryModel Query { get; set; }

			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				if (node.Method.DeclaringType == typeof (SpQueryable))
				{
					if (!MethodUtils.IsOperator(node.Method, MethodUtils.SpqFakeFetch))
					{
						Query = (QueryModel) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value;
					}
					else
					{
						Query = null;
					}
				}
				return base.VisitMethodCall(node);
			}
		}

		protected class SpQueryRemover : ExpressionVisitor
		{
			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				if (node.Method.DeclaringType == typeof (SpQueryable))
				{
					return Expression.Constant(null, node.Type);
				}

				if (!MethodUtils.IsOperator(node.Method, MethodUtils.QAsQueryable))
				{
					return base.VisitMethodCall(node);
				}

				var source = Visit(node.Arguments[0]);

				return source.IsConstant(null)
					? (Expression) Expression.Constant(null, node.Type)
					: node.Update(null, new[] {source});
			}
		}
	}
}
