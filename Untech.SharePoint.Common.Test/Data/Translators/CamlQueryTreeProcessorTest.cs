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
	public class CamlQueryTreeProcessorTest : BaseExpressionTest
	{
		[TestMethod]
		public void CanBeRun()
		{
			Given(source => source)
				.ExpectedCaml("<Query></Query>");
		}

		[TestMethod]
		public void CanProcessSingleWhere()
		{
			Given(source => source.Where(n => n.Bool1 && n.Int1 > 10 || n.String1.StartsWith("TEST")))
				.ExpectedCaml("<Query>" +
							  "<Where><Or><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></Or></Where>" +
							  "</Query>");
		}

		[TestMethod]
		public void CanProcessMultipleWhere()
		{
			Given(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>");
		}

		[TestMethod]
		public void CanProcessOrderBy()
		{
			Given(source => source.OrderBy(n => n.Bool1).ThenBy(n => n.String1))
				.ExpectedCaml("<Query>" +
							  "<OrderBy><FieldRef Name='Bool1' Ascending='TRUE' /><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
							  "</Query>");
		}

		[TestMethod]
		public void CanProcessOrderByAndReverse()
		{
			Given(source => source.OrderBy(n => n.Bool1).ThenBy(n => n.String1).Reverse())
				.ExpectedCaml("<Query>" +
							  "<OrderBy><FieldRef Name='Bool1' Ascending='FALSE' /><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>");
		}

		[TestMethod]
		public void CanProcessTake()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Where(n => n.String1.StartsWith("TEST"))
				.Take(10))
				.ExpectedCaml("<Query><RowLimit>10</RowLimit>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>");
		}

		[TestMethod]
		public void CanProcessTakeAndOuterCalls()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Where(n => n.String1.StartsWith("TEST"))
				.Take(10)
				.Where(n => n.String1 == "SOME"))
				.ExpectedCaml(
					"<Query><RowLimit>10</RowLimit>" +
					"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
					"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
					"</Query>").ExprectedQueryTree(source => source.Where(n => n.String1 == "SOME"));
		}

		[TestMethod]
		public void CanProcessSkip()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Where(n => n.String1.StartsWith("TEST"))
				.Skip(10))
				.ExpectedCaml(
					"<Query>" +
					"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
					"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
					"</Query>");
		}

		[TestMethod]
		public void CanProcessSkipAndOuterCalls()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Where(n => n.String1.StartsWith("TEST"))
				.Skip(10)
				.Take(1))
				.ExpectedCaml("<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>")
				.ExprectedQueryTree(source => source.Take(1));
		}

		[TestMethod]
		public void CanProcessAny()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Any())
				.ExpectedCaml("<Query>" +
							  "<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
							  "</Query>");
		}


		[TestMethod]
		public void CanProcessAnyPredicate()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Any(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>");
		}

		[TestMethod]
		public void CanProcessAll()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.All(n => n.String1 == "TEST"))
				.ExpectedCaml("<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<Neq><FieldRef Name='String1' /><Value>TEST</Value></Neq></And></Where>" +
							  "</Query>");

			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Any(n => n.String1 != "TEST"))
				.ExpectedCaml("<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<Neq><FieldRef Name='String1' /><Value>TEST</Value></Neq></And></Where>" +
							  "</Query>");
		}

		[TestMethod]
		public void CanProcessSelect()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new { Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1 })
				.Any())
				.ExpectedCaml("<Query>" +
							  "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
							  "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
							  "</Query>");
		}


		[TestMethod]
		public void ThrowForInvalidAll()
		{
			CustomAssert.Throw<NotSupportedException>(() =>
			{
				// NOTE: unable to process '!n.String1.StartsWith("TEST")'
				Given(source => source
					.Where(n => n.Bool1)
					.Where(n => n.Int1 > 10)
					.All(n => n.String1.StartsWith("TEST")))
					.ExpectedCaml("");
			});
		}

		[TestMethod]
		public void CanProcessFirst()
		{
			Given(source => source.OrderBy(n => n.String1).First(n => n.String2 == "TEST"))
				.ExpectedCaml("<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
							  "</Query>");

		}

		[TestMethod]
		public void CanProcessSingle()
		{
			Given(source => source.OrderBy(n => n.String1).Single(n => n.String2 == "TEST"))
				.ExpectedCaml("<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
							  "</Query>");

		}

		[TestMethod]
		public void CanProcessLast()
		{
			Given(source => source.OrderBy(n => n.String1).Last(n => n.String2 == "TEST"))
				.ExpectedCaml("<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>");

		}

		[TestMethod]
		public void CanProcessLastWithOrderBy()
		{
			Given(source => source.OrderBy(n => n.String1).Last(n => n.String2 == "TEST"))
				.ExpectedCaml("<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>");

		}

		[TestMethod]
		public void CanProcessReverse()
		{
			Given(source => source.OrderBy(n => n.String1).Reverse())
				.ExpectedCaml("<Query>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>");

		}

		[TestMethod]
		public void CanProcessReverseAndOuterCalls()
		{
			Given(
				source => source.OrderBy(n => n.String1).ThenByDescending(n => n.String2).Reverse().Where(n => n.String2 == "TEST"))
				.ExpectedCaml("<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /><FieldRef Name='String2' Ascending='TRUE' /></OrderBy>" +
							  "</Query>");

		}

		public class TestScenario
		{
			private readonly Expression _given;

			public TestScenario(Expression given)
			{
				_given = given;
			}

			public TestScenario ExpectedCaml(string expectedCaml)
			{
				var queryModel = QueryFinder.FindQuery(_given) ?? new QueryModel();

				Assert.AreEqual(expectedCaml, queryModel.ToString());

				return this;
			}


			public TestScenario ExprectedQueryTree(Func<IQueryable<Entity>, IQueryable<Entity>> expectedQuery)
			{
				var actualTree = new SpQueryRemover().Visit(_given);
				var expectedQueryTree = expectedQuery(new FakeQueryable<Entity>()).Expression;

				expectedQueryTree = new SpQueryRemover().Visit(expectedQueryTree);

				Assert.AreEqual(expectedQueryTree.ToString(), actualTree.ToString());

				return this;
			}

			public TestScenario ExprectedQueryTree(Func<IQueryable<Entity>, object> expectedQuery)
			{
				var actualTree = new SpQueryRemover().Visit(_given);
				Expression expectedQueryTree = null;
				expectedQuery(new FakeQueryable<Entity>
				{
					ExpressionExecutor = node => { expectedQueryTree = node; }
				});

				expectedQueryTree = new SpQueryRemover().Visit(expectedQueryTree);

				Assert.AreEqual(expectedQueryTree.ToString(), actualTree.ToString());

				return this;
			}
		}

		protected TestScenario Given(Func<IQueryable<Entity>, IQueryable<Entity>> originalQuery)
		{
			var originalQueryTree = originalQuery(new FakeQueryable<Entity>()).Expression;
			originalQueryTree = new CamlQueryTreeProcessor().Process(originalQueryTree);

			return new TestScenario(originalQueryTree);
		}

		protected TestScenario Given(Func<IQueryable<Entity>, object> originalQuery)
		{
			Expression originalQueryTree = null;
			originalQuery(new FakeQueryable<Entity>
			{
				ExpressionExecutor = node => { originalQueryTree = node; }
			});
			originalQueryTree = new CamlQueryTreeProcessor().Process(originalQueryTree);

			return new TestScenario(originalQueryTree);
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
				if (node.Method.DeclaringType == typeof(SpQueryable))
				{
					if (!MethodUtils.IsOperator(node.Method, MethodUtils.SpqFakeFetch))
					{
						Query = (QueryModel)((ConstantExpression)node.Arguments[1].StripQuotes()).Value;
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
				if (node.Method.DeclaringType == typeof(SpQueryable))
				{
					if (node.Type.Is<bool>())
					{
						return Expression.Constant(true);
					}
					return Expression.Constant(null, node.Type);
				}

				if (!MethodUtils.IsOperator(node.Method, MethodUtils.QAsQueryable))
				{
					return base.VisitMethodCall(node);
				}

				var source = Visit(node.Arguments[0]);

				return source.IsConstant(null)
					? (Expression)Expression.Constant(null, node.Type)
					: node.Update(null, new[] { source });
			}
		}
	}
}
