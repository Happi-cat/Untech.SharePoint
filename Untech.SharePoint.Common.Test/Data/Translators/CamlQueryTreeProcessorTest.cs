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
				.ExpectedCaml("<View><Query></Query></View>");
		}

		[TestMethod]
		public void CanProcessSingleWhere()
		{
			Given(source => source.Where(n => n.Bool1 && n.Int1 > 10 || n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><Or><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></Or></Where>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessMultipleWhere()
		{
			Given(source => source.Where(n => n.Bool1).Where(n => n.Int1 > 10).Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessAlwaysTrueWhere()
		{
			Given(source => source.Where(n => true).Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><IsNotNull><FieldRef Name='ID' /></IsNotNull>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessAlwaysFalseWhere()
		{
			Given(source => source.Where(n => false).Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><IsNull><FieldRef Name='ID' /></IsNull>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessXorWhere()
		{
			Given(source => source.Where(n => n.Bool1 ^ n.Bool2).Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And>" +
							  "<Or><And>" +
							  "<Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq>" +
							  "<Eq><FieldRef Name='Bool2' /><Value>False</Value></Eq>" +
							  "</And><And>" +
							  "<Eq><FieldRef Name='Bool1' /><Value>False</Value></Eq>" +
							  "<Eq><FieldRef Name='Bool2' /><Value>True</Value></Eq>" +
							  "</And></Or>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessOrderBy()
		{
			Given(source => source.OrderBy(n => n.Bool1).ThenBy(n => n.String1))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<OrderBy><FieldRef Name='Bool1' Ascending='TRUE' /><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessOrderByAndReverse()
		{
			Given(source => source.OrderBy(n => n.Bool1).ThenBy(n => n.String1).Reverse())
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<OrderBy><FieldRef Name='Bool1' Ascending='FALSE' /><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessTake()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Where(n => n.String1.StartsWith("TEST"))
				.Take(10))
				.ExpectedCaml("<View>" +
							  "<RowLimit>10</RowLimit>" +
							  "<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>" +
							  "</View>");
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
				.ExpectedCaml("<View>" +
							  "<RowLimit>10</RowLimit>" +
					"<Query>" +
					"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
					"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
					"</Query>" +
							  "</View>")
				.ExprectedQueryTree(source => source.Where(n => n.String1 == "SOME"));
		}

		[TestMethod]
		public void CanProcessSkip()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Where(n => n.String1.StartsWith("TEST"))
				.Skip(10))
				.ExpectedCaml("<View>" +
					"<Query>" +
					"<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
					"<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
					"</Query>" +
							  "</View>");
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
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>" +
							  "</View>")
				.ExprectedQueryTree(source => source.Take(1));
		}

		[TestMethod]
		public void CanProcessAny()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Any())
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
							  "</Query>" +
							  "</View>");
		}


		[TestMethod]
		public void CanProcessAnyPredicate()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Any(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessAll()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.All(n => n.String1 == "TEST"))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<Neq><FieldRef Name='String1' /><Value>TEST</Value></Neq></And></Where>" +
							  "</Query>" +
							  "</View>");

			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Any(n => n.String1 != "TEST"))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
							  "<Neq><FieldRef Name='String1' /><Value>TEST</Value></Neq></And></Where>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void CanProcessSelect()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new { Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1 })
				.Any())
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
							  "</Query>" +
							  "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
							  "</View>");
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
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");

		}

		[TestMethod]
		public void CanProcessSingle()
		{
			Given(source => source.OrderBy(n => n.String1).Single(n => n.String2 == "TEST"))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");

		}

		[TestMethod]
		public void CanProcessLast()
		{
			Given(source => source.OrderBy(n => n.String1).Last(n => n.String2 == "TEST"))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");

		}

		[TestMethod]
		public void CanProcessLastWithOrderBy()
		{
			Given(source => source.OrderBy(n => n.String1).Last(n => n.String2 == "TEST"))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");

		}

		[TestMethod]
		public void CanProcessReverse()
		{
			Given(source => source.OrderBy(n => n.String1).Reverse())
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");

		}

		[TestMethod]
		public void CanProcessReverseAndOuterCalls()
		{
			Given(source => source
				.OrderBy(n => n.String1)
				.ThenByDescending(n => n.String2)
				.Reverse()
				.Where(n => n.String2 == "TEST"))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
							  "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /><FieldRef Name='String2' Ascending='TRUE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");

		}

		protected TestScenario Given(Func<IQueryable<Entity>, IQueryable<Entity>> originalQuery)
		{
			var originalQueryTree = new CamlQueryTreeProcessor().Process(GetExpression(originalQuery));

			return new TestScenario(originalQueryTree);
		}

		protected TestScenario Given(Func<IQueryable<Entity>, object> originalQuery)
		{
			var originalQueryTree = new CamlQueryTreeProcessor().Process(GetExpression(originalQuery));

			return new TestScenario(originalQueryTree);
		}

		private static Expression GetExpression(Func<IQueryable<Entity>, IQueryable<Entity>> query)
		{
			return query(new FakeQueryable<Entity>()).Expression;
		}

		private static Expression GetExpression(Func<IQueryable<Entity>, object> query)
		{
			Expression queryTree = null;
			query(new FakeQueryable<Entity>
			{
				ExpressionExecutor = node => { queryTree = node; }
			});
			return queryTree;
		}

		#region [Nested Classes]

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
				var expectedQueryTree = GetExpression(expectedQuery);
				ExpectedExpression(expectedQueryTree);
				return this;
			}

			public TestScenario ExprectedQueryTree(Func<IQueryable<Entity>, object> expectedQuery)
			{
				var expectedQueryTree = GetExpression(expectedQuery);
				ExpectedExpression(expectedQueryTree);
				return this;
			}

			private void ExpectedExpression(Expression expectedExpression)
			{
				var givenExpression = new SpQueryRemover().Visit(_given);
				expectedExpression = new SpQueryRemover().Visit(expectedExpression);

				Assert.AreEqual(expectedExpression.ToString(), givenExpression.ToString());
			}
		}

		public class QueryFinder : ExpressionVisitor
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

		public class SpQueryRemover : ExpressionVisitor
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

		#endregion

	}
}
