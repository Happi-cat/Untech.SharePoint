using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.Predicate;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Test.Data.Translators.Predicate
{
	[TestClass]
	public class CamlPredicateProcessorTest : BaseExpressionTest
	{
		[TestMethod]
		public void CanConvert()
		{
			Given(n => n.String1.Contains("1") && n.Int1 == 2)
				.Expected("<And>" +
						  "<Contains><FieldRef Name='String1' /><Value>1</Value></Contains>" +
						  "<Eq><FieldRef Name='Int1' /><Value>2</Value></Eq>" +
						  "</And>");
		}

		[TestMethod]
		public void SupportIsNullOrEmpty()
		{
			Given(n => string.IsNullOrEmpty(n.String1))
				.Expected("<Or>" +
						  "<IsNull><FieldRef Name='String1' /></IsNull>" +
						  "<Eq><FieldRef Name='String1' /><Value></Value></Eq>" +
						  "</Or>");
		}

		[TestMethod]
		public void SupportStartsWith()
		{
			Given(n => n.String1.StartsWith("START"))
				.Expected("<BeginsWith><FieldRef Name='String1' /><Value>START</Value></BeginsWith>");
		}

		[TestMethod]
		public void SupportXorForBooleans()
		{
			Given(n => n.Bool1 ^ n.Bool2)
				.Expected("<Or>" +
				"<And>" +
				"<Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq>" +
				"<Eq><FieldRef Name='Bool2' /><Value>False</Value></Eq>" +
				"</And>" +
				"<And>" +
				"<Eq><FieldRef Name='Bool1' /><Value>False</Value></Eq>" +
				"<Eq><FieldRef Name='Bool2' /><Value>True</Value></Eq>" +
				"</And>" +
				"</Or>");
		}

		[TestMethod]
		public void NotSupportXorForCalls()
		{
			CustomAssert.Throw<NotSupportedException>(() => Given(n => n.String1.StartsWith("START") ^ n.Bool2).Expected("UNDEFINED"));
		}


		[TestMethod]
		public void SupportInEnumerable()
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
		public void SupportEnumerableAndListContains()
		{
			var possibleValues1 = new List<int> { 1, 2, 3 };
			var possibleValues2 = (IEnumerable<int>) possibleValues1;

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
		public void SupportBoolProperties()
		{
			Given(n => n.Bool1 && !n.Bool2)
				.Expected("<And>" +
						  "<Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq>" +
						  "<Eq><FieldRef Name='Bool2' /><Value>False</Value></Eq>" +
						  "</And>");
		}

		[TestMethod]
		public void SupportStringEquality()
		{
			Given(n => n.String1 == "TEST").Expected("<Eq><FieldRef Name='String1' /><Value>TEST</Value></Eq>");

			Given(n => "TEST" == n.String1).Expected("<Eq><FieldRef Name='String1' /><Value>TEST</Value></Eq>");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
		[SuppressMessage("ReSharper", "EqualExpressionComparison")]
		public void CanSwapMemberToLeft()
		{
			Given(n => n.Int1 == 1 && 2 == n.Int2)
				.Expected("<And><Eq><FieldRef Name='Int1' /><Value>1</Value></Eq><Eq><FieldRef Name='Int2' /><Value>2</Value></Eq></And>");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
		[SuppressMessage("ReSharper", "EqualExpressionComparison")]
		public void CanOptimizeConditions()
		{
			Given(n => n.Int1 == 1 && 3 == 3 && true)
				.Expected("<Eq><FieldRef Name='Int1' /><Value>1</Value></Eq>");

			Given(n => (n.Int1 == 1 && 3 == 3) || true)
				.Expected("<IsNotNull><FieldRef Name='ID' /></IsNotNull>");

			Given(n => n.Int1 == 1 && false)
				.Expected("<IsNull><FieldRef Name='ID' /></IsNull>");
		}

		[TestMethod]
		public void ThrowIfInvalid()
		{
			CustomAssert.Throw<NotSupportedException>(() => Given(n => n.String1.Contains(n.String2)).Expected("SHOULD THROW"));
			CustomAssert.Throw<NotSupportedException>(() => Given(n => n.Bool1 == n.Bool2).Expected("SHOULD THROW"));
		}

		protected TestScenario Given(Expression<Func<Entity, bool>> given)
		{
			return new TestScenario(given);
		}

		#region [Nested Classes]

		public class TestScenario
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