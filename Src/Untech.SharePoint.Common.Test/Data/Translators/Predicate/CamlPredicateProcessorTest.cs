using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	[TestClass]
	public class CamlPredicateProcessorTest : BaseExpressionTest
	{
		[TestMethod]
		public void Process_GenerateCaml_WhenSimpleQuery()
		{
			Given(n => n.String1.Contains("1") && n.Int1 == 2)
				.Expected("<And>" +
						  "<Contains><FieldRef Name='String1' /><Value>1</Value></Contains>" +
						  "<Eq><FieldRef Name='Int1' /><Value>2</Value></Eq>" +
						  "</And>");
		}

		[TestMethod]
		public void Process_GenerateCaml_WhenStringIsNullOrEmpty()
		{
			Given(n => string.IsNullOrEmpty(n.String1))
				.Expected("<Or>" +
						  "<IsNull><FieldRef Name='String1' /></IsNull>" +
						  "<Eq><FieldRef Name='String1' /><Value></Value></Eq>" +
						  "</Or>");
		}

		[TestMethod]
		public void Process_GenerateCaml_StringStartsWith()
		{
			Given(n => n.String1.StartsWith("START"))
				.Expected("<BeginsWith><FieldRef Name='String1' /><Value>START</Value></BeginsWith>");
		}

		[TestMethod]
		public void Process_GenerateCaml_WhenXorBetweenProps()
		{
			Given(n => n.Bool1 ^ n.Bool2)
				.Expected("<Or>" +
				"<And>" +
				"<Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq>" +
				"<Neq><FieldRef Name='Bool2' /><Value>True</Value></Neq>" +
				"</And>" +
				"<And>" +
				"<Neq><FieldRef Name='Bool1' /><Value>True</Value></Neq>" +
				"<Eq><FieldRef Name='Bool2' /><Value>True</Value></Eq>" +
				"</And>" +
				"</Or>");
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void Process_ThrowNotSupport_WhenXorBetweenPropAndCall()
		{
			Given(n => n.String1.StartsWith("START") ^ n.Bool2).Expected("UNDEFINED");
		}


		[TestMethod]
		public void Process_GenerateCaml_WhenEnumerableIn()
		{
			var possibleValues = new[] { 1, 2, 3 };

			Given(n => n.Int1.In(possibleValues) && !n.Int2.In(possibleValues))
				.Expected("<And>" +
						  "<Or>" +
						  "<Or><Eq><FieldRef Name='Int1' /><Value>1</Value></Eq><Eq><FieldRef Name='Int1' /><Value>2</Value></Eq></Or>" +
						  "<Eq><FieldRef Name='Int1' /><Value>3</Value></Eq>" +
						  "</Or>" +
						  "<And>" +
						  "<And><Neq><FieldRef Name='Int2' /><Value>1</Value></Neq><Neq><FieldRef Name='Int2' /><Value>2</Value></Neq></And>" +
						  "<Neq><FieldRef Name='Int2' /><Value>3</Value></Neq>" +
						  "</And>" +
						  "</And>");
		}

		[TestMethod]
		public void Process_GenerateCaml_WhenEnumerableAndListContains()
		{
			var possibleValues1 = new List<int> { 1, 2, 3 };
			var possibleValues2 = (IEnumerable<int>)possibleValues1;

			Given(n => possibleValues1.Contains(n.Int1) && !possibleValues2.Contains(n.Int2))
				.Expected("<And>" +
						  "<Or>" +
						  "<Or><Eq><FieldRef Name='Int1' /><Value>1</Value></Eq><Eq><FieldRef Name='Int1' /><Value>2</Value></Eq></Or>" +
						  "<Eq><FieldRef Name='Int1' /><Value>3</Value></Eq>" +
						  "</Or>" +
						  "<And>" +
						  "<And><Neq><FieldRef Name='Int2' /><Value>1</Value></Neq><Neq><FieldRef Name='Int2' /><Value>2</Value></Neq></And>" +
						  "<Neq><FieldRef Name='Int2' /><Value>3</Value></Neq>" +
						  "</And>" +
						  "</And>");
		}

		[TestMethod]
		public void Process_GenerateCaml_WhenCollectionContainsAndNotContains()
		{
			Given(n => n.StringCollection1.Contains("Value 1") && !n.StringCollection4.Contains("Value 4"))
				.Expected("<And>" +
						  "<ContainsOrIncludes><FieldRef Name='StringCollection1' /><Value>Value 1</Value></ContainsOrIncludes>" +
						  "<NotContainsOrIncludes><FieldRef Name='StringCollection4' /><Value>Value 4</Value></NotContainsOrIncludes>" +
						  "</And>");
		}

		[TestMethod]
		public void Process_GenerateCaml_WhenBoolProps()
		{
			Given(n => n.Bool1 && !n.Bool2)
				.Expected("<And>" +
						  "<Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq>" +
						  "<Neq><FieldRef Name='Bool2' /><Value>True</Value></Neq>" +
						  "</And>");
		}

		[TestMethod]
		public void Process_GenerateCaml_WhenStringEquality()
		{
			Given(n => n.String1 == "TEST").Expected("<Eq><FieldRef Name='String1' /><Value>TEST</Value></Eq>");

			Given(n => "TEST" == n.String1).Expected("<Eq><FieldRef Name='String1' /><Value>TEST</Value></Eq>");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "EqualExpressionComparison")]
		public void Process_SwapMemberToLeft()
		{
			Given(n => n.Int1 == 1 && 2 == n.Int2)
				.Expected("<And><Eq><FieldRef Name='Int1' /><Value>1</Value></Eq><Eq><FieldRef Name='Int2' /><Value>2</Value></Eq></And>");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
		[SuppressMessage("ReSharper", "EqualExpressionComparison")]
		public void Process_OptimizeConditions()
		{
			Given(n => n.Int1 == 1 && 3 == 3 && true)
				.Expected("<Eq><FieldRef Name='Int1' /><Value>1</Value></Eq>");

			Given(n => (n.Int1 == 1 && 3 == 3) || true)
				.Expected("<IsNotNull><FieldRef Name='ID' /></IsNotNull>");

			Given(n => n.Int1 == 1 && false)
				.Expected("<IsNull><FieldRef Name='ID' /></IsNull>");
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void Process_ThrowNotSupported_WhenPropStringContainsProp()
		{
			Given(n => n.String1.Contains(n.String2)).Expected("SHOULD THROW");
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void Process_ThrowNotSupported_WhenPropComparedToProp()
		{
			Given(n => n.Bool1 == n.Bool2).Expected("SHOULD THROW");
		}

		private TestScenario Given(Expression<Func<Entity, bool>> given)
		{
			return new TestScenario(given);
		}

		#region [Nested Classes]

		private class TestScenario
		{
			private readonly Expression<Func<Entity, bool>> _given;

			public TestScenario(Expression<Func<Entity, bool>> given)
			{
				_given = given;
			}

			public void Expected(string expected)
			{
				var processor = new CamlPredicateProcessor();

				Assert.AreEqual(expected, processor.Process(_given).ToString());
			}
		}

		#endregion
	}
}