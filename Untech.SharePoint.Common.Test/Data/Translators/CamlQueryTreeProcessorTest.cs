using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.CodeAnnotations;
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
		public void SupportDefault()
		{
			Given(source => source).ExpectedCaml("<View><Query></Query></View>");
		}

		#region [Where]

		[TestMethod]
		public void SupportWhere()
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
		public void SupportWhereWhere()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<Where><And><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And>" +
				              "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportWhereTrue()
		{
			Given(source => source
				.Where(n => true)
				.Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<Where><And><IsNotNull><FieldRef Name='ID' /></IsNotNull>" +
				              "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportWhereFalse()
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
		public void SupportWhereXor()
		{
			Given(source => source
				.Where(n => n.Bool1 ^ n.Bool2)
				.Where(n => n.String1.StartsWith("TEST")))
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<Where><And>" +
				              "<Or><And>" +
				              "<Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq>" +
				              "<Neq><FieldRef Name='Bool2' /><Value>True</Value></Neq>" +
				              "</And><And>" +
				              "<Neq><FieldRef Name='Bool1' /><Value>True</Value></Neq>" +
				              "<Eq><FieldRef Name='Bool2' /><Value>True</Value></Eq>" +
				              "</And></Or>" +
				              "<BeginsWith><FieldRef Name='String1' /><Value>TEST</Value></BeginsWith></And></Where>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportWhereArrayContains()
		{
			Given(source => source
				.Where(n => n.StringCollection1.Contains("TEST")))
				.ExpectedCaml("<View><Query><Where>" +
				              "<Includes><FieldRef Name='StringCollection1' /><Value>TEST</Value></Includes>" +
				              "</Where></Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportWhereEnumerableContains()
		{
			Given(source => source
				.Where(n => n.StringCollection2.Contains("TEST")))
				.ExpectedCaml("<View><Query><Where>" +
				              "<Includes><FieldRef Name='StringCollection2' /><Value>TEST</Value></Includes>" +
				              "</Where></Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportWhereCollectionContains()
		{
			Given(source => source
				.Where(n => n.StringCollection3.Contains("TEST")))
				.ExpectedCaml("<View><Query><Where>" +
				              "<Includes><FieldRef Name='StringCollection3' /><Value>TEST</Value></Includes>" +
				              "</Where></Query>" +
				              "</View>");

		}

		[TestMethod]
		public void SupportWhereListContains()
		{
			Given(source => source
				.Where(n => n.StringCollection4.Contains("TEST")))
				.ExpectedCaml("<View><Query><Where>" +
				              "<Includes><FieldRef Name='StringCollection4' /><Value>TEST</Value></Includes>" +
				              "</Where></Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportWhereTake()
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

		#endregion


		#region [Order By, Then By]

		[TestMethod]
		public void SupportOrderByThenBy()
		{
			Given(source => source
				.OrderBy(n => n.Bool1)
				.ThenBy(n => n.String1))
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<OrderBy><FieldRef Name='Bool1' Ascending='TRUE' /><FieldRef Name='String1' Ascending='TRUE' /></OrderBy>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportOrderByDescThenByDesc()
		{
			Given(source => source
				.OrderByDescending(n => n.Bool1)
				.ThenByDescending(n => n.String1))
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<OrderBy><FieldRef Name='Bool1' Ascending='FALSE' /><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
							  "</Query>" +
							  "</View>");
		}

		[TestMethod]
		public void SupportOrderByThenByReverse()
		{
			Given(source => source
				.OrderBy(n => n.Bool1)
				.ThenBy(n => n.String1)
				.Reverse())
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<OrderBy><FieldRef Name='Bool1' Ascending='FALSE' /><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportOrderByLast()
		{
			Given(source => source.OrderBy(n => n.String1).Last(n => n.String2 == "TEST"))
				.ExpectedCaml("<View>" +
				              "<RowLimit>1</RowLimit>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
				              "<OrderBy><FieldRef Name='String1' Ascending='FALSE' /></OrderBy>" +
				              "</Query>" +
				              "</View>");
		}

		#endregion


		#region [Any]

		[TestMethod]
		public void SupportAny()
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
		public void SupportAnyP()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Any(n => n.Int1 > 10))
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
				              "</Query>" +
				              "</View>");
		}

		#endregion


		#region [All]

		[TestMethod]
		public void SupportAll()
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
		}

		#endregion


		#region [Select]

		[TestMethod]
		public void SupportSelect()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new {Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1}))
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
				              "</Query>" +
				              "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportSelectAny()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Select(n => n.String1)
				.Any())
				.ExpectedCaml("<View>" +
							  "<Query>" +
							  "<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
							  "</Query>" +
							  "<ViewFields><FieldRef Name='String1' /></ViewFields>" +
							  "</View>");
		}

		[TestMethod]
		public void SupportSelectTake()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new {Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1})
				.Take(10))
				.ExpectedCaml("<View>" +
				              "<RowLimit>10</RowLimit>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
				              "</Query>" +
				              "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
				              "</View>");
		}

		[TestMethod]
		public void NotSupportSelectSelect()
		{
			CustomAssert.Throw<InvalidOperationException>(
				() => Given(source => source
					.Select(n => new { Value = n.Int1 })
					.Select(n => n.Value)));
		}

		[TestMethod]
		public void NotSupportSelectAnyP()
		{
			CustomAssert.Throw<InvalidOperationException>(
				() => Given(source => source
					.Select(n => n.Int1)
					.Any(n => n > 10)));
		}


		[TestMethod]
		public void NotSupportSelectAll()
		{
			CustomAssert.Throw<InvalidOperationException>(
				() => Given(source => source
					.Select(n => n.String1)
					.All(n => n == "TEST")));
		}


		[TestMethod]
		public void SupportSelectFirst()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new {Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1})
				.First())
				.ExpectedCaml("<View>" +
				              "<RowLimit>1</RowLimit>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
				              "</Query>" +
				              "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
				              "</View>");
		}

		[TestMethod]
		public void NotSupportSelectFirstP()
		{
			CustomAssert.Throw<InvalidOperationException>(() =>
				Given(source => source
					.Select(n => new {Result = n.Int1})
					.First(n => n.Result > 10)));
		}

		[TestMethod]
		public void SupportSelectFirstOrDefault()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new {Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1})
				.FirstOrDefault())
				.ExpectedCaml("<View>" +
				              "<RowLimit>1</RowLimit>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
				              "</Query>" +
				              "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
				              "</View>");
		}

		[TestMethod]
		public void NotSupportSelectFirstOrDefaultP()
		{
			CustomAssert.Throw<InvalidOperationException>(() =>
				Given(source => source
					.Select(n => new {Result = n.Int1})
					.FirstOrDefault(n => n.Result > 10)));
		}


		[TestMethod]
		public void SupportSelectLast()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new {Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1})
				.Last())
				.ExpectedCaml("<View>" +
				              "<RowLimit>1</RowLimit>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
				              "</Query>" +
				              "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
				              "</View>");
		}

		[TestMethod]
		public void NotSupportSelectLastP()
		{
			CustomAssert.Throw<InvalidOperationException>(() =>
				Given(source => source
					.Select(n => new {Result = n.Int1})
					.Last(n => n.Result > 10)));
		}


		[TestMethod]
		public void NotSupportSelectWhere()
		{
			CustomAssert.Throw<InvalidOperationException>(() =>
				Given(source => source
					.Select(n => new { Result = n.Int1 })
					.Where(n => n.Result > 10)));
		}

		[TestMethod]
		public void SupportSelectLastOrDefault()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Select(n => new {Result = n.Int1, Result2 = n.String1, Result3 = n.Int1 + n.String1})
				.LastOrDefault())
				.ExpectedCaml("<View>" +
				              "<RowLimit>1</RowLimit>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq></Where>" +
				              "</Query>" +
				              "<ViewFields><FieldRef Name='Int1' /><FieldRef Name='String1' /></ViewFields>" +
				              "</View>");
		}

		[TestMethod]
		public void NotSupportSelectLastOrDefaultP()
		{
			CustomAssert.Throw<InvalidOperationException>(() =>
				Given(source => source
					.Select(n => new {Result = n.Int1})
					.LastOrDefault(n => n.Result > 10)));
		}

		#endregion


		#region [Take]

		[TestMethod]
		public void SupportTake()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Take(10))
				.ExpectedCaml("<View>" +
				              "<RowLimit>10</RowLimit>" +
				              "<Query>" +
				              "<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportTakeAny()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Take(10)
				.Any())
				.ExpectedCaml("<View>" +
				              "<RowLimit>10</RowLimit>" +
				              "<Query>" +
				              "<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void SupportTakeFirst()
		{
			Given(source => source
				.Where(n => n.Bool1)
				.Where(n => n.Int1 > 10)
				.Take(10)
				.First())
				.ExpectedCaml("<View>" +
				              "<RowLimit>1</RowLimit>" +
				              "<Query>" +
				              "<Where><And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Gt><FieldRef Name='Int1' /><Value>10</Value></Gt></And></Where>" +
				              "</Query>" +
				              "</View>");
		}

		[TestMethod]
		public void NotSupportTakeLast()
		{
			CustomAssert.Throw<InvalidOperationException>(() => Given(source => source.Take(10).Last()));
		}

		[TestMethod]
		public void NotSupportTakeWhere()
		{
			CustomAssert.Throw<InvalidOperationException>(() => Given(source => source.Take(10).Where(n => n.Bool1)));
		}

		[TestMethod]
		public void NotSupportTakeOrderBy()
		{
			CustomAssert.Throw<InvalidOperationException>(() => Given(source => source.Take(10).OrderBy(n => n.Bool1)));
		}

		[TestMethod]
		public void NotSupportTakeAll()
		{
			CustomAssert.Throw<InvalidOperationException>(() => Given(source => source.Take(10).All(n => n.String1 == "TEST")));
		}

		#endregion


		#region [Reverse]

		[TestMethod]
		public void SupportReverseWhere()
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

		[TestMethod]
		public void SupportReverseOrderByThenBy()
		{
			Given(source => source
				.Reverse()
				.OrderBy(n => n.String1)
				.ThenByDescending(n => n.String2)
				.Where(n => n.String2 == "TEST"))
				.ExpectedCaml("<View>" +
				              "<Query>" +
				              "<Where><Eq><FieldRef Name='String2' /><Value>TEST</Value></Eq></Where>" +
				              "<OrderBy><FieldRef Name='String1' Ascending='TRUE' /><FieldRef Name='String2' Ascending='FALSE' /></OrderBy>" +
				              "</Query>" +
				              "</View>");

		}

		#endregion


		#region [Other]

		[TestMethod]
		public void NotSupportSkip()
		{
			CustomAssert.Throw<NotSupportedException>(() => Given(source => source.Skip(10)));
		}

		[TestMethod]
		public void NotSupportStrStartsWithNegate()
		{
			CustomAssert.Throw<NotSupportedException>(() =>
			{
				// NOTE: unable to process '!n.String1.StartsWith("TEST")'
				Given(source => source.All(n => n.String1.StartsWith("TEST")));
			});
		}

		#endregion
		

		#region [Private Methods]

		private TestScenario Given<T>(Func<IQueryable<Entity>, IQueryable<T>> originalQuery)
		{
			var originalQueryTree = new CamlQueryTreeProcessor().Process(GetExpression(originalQuery));

			return new TestScenario(originalQueryTree);
		}

		private TestScenario Given(Func<IQueryable<Entity>, object> originalQuery)
		{
			var originalQueryTree = new CamlQueryTreeProcessor().Process(GetExpression(originalQuery));

			return new TestScenario(originalQueryTree);
		}

		private static Expression GetExpression<T>(Func<IQueryable<Entity>, IQueryable<T>> query)
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

		#endregion
		

		#region [Nested Classes]

		[PublicAPI]
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

			public TestScenario ExprectedQueryTree<T>(Func<IQueryable<Entity>, IQueryable<T>> expectedQuery)
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

		private class QueryFinder : ExpressionVisitor
		{
			public static QueryModel FindQuery(Expression node)
			{
				var finder = new QueryFinder();

				finder.Visit(node);

				return finder.Query;
			}

			private QueryModel Query { get; set; }

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

		private class SpQueryRemover : ExpressionVisitor
		{
			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				if (node.Method.DeclaringType == typeof (SpQueryable))
				{
					if (node.Type == typeof (bool))
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
					? (Expression) Expression.Constant(null, node.Type)
					: node.Update(null, new[] {source});
			}
		}

		#endregion

	}
}
