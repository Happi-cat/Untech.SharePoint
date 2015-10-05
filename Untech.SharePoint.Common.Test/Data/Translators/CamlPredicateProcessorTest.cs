using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators;
using Untech.SharePoint.Common.Data.Translators.Predicate;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	[TestClass]
	public class CamlPredicateProcessorTest
	{
		[TestMethod]
		public void CanConvert()
		{
			Test(n => n.String1.Contains("1") && n.Int1 == 2, "<And><Contains><FieldRef Name='String1' /><Value>1</Value></Contains><Eq><FieldRef Name='Int1' /><Value>2</Value></Eq></And>");
		}

		[TestMethod]
		public void SupportIsNullOrEmpty()
		{
			Test(n => string.IsNullOrEmpty(n.String1), "<Or><IsNull><FieldRef Name='String1' /></IsNull><Eq><FieldRef Name='String1' /><Value></Value></Eq></Or>");
		}

		[TestMethod]
		public void SupportStartsWith()
		{
			Test(n => n.String1.StartsWith("START"), "<BeginsWith><FieldRef Name='String1' /><Value>START</Value></BeginsWith>");
		}

		[TestMethod]
		public void SupportEnumerableContains()
		{
			var possibleValues = new[] { 1, 2, 3 };
			Test(n => n.Int1.In(possibleValues) && !n.Int2.In(possibleValues), "<And>" +
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
		public void SupportNotAndBoolProperties()
		{
			Test(n => n.Bool1 && !n.Bool2, "<And><Eq><FieldRef Name='Bool1' /><Value>True</Value></Eq><Eq><FieldRef Name='Bool2' /><Value>False</Value></Eq></And>");
		}

		[TestMethod]
		public void SupportStringEquality()
		{
			Test(n => n.String1 == "TEST", "<Eq><FieldRef Name='String1' /><Value>TEST</Value></Eq>");
			Test(n => "TEST" == n.String1, "<Eq><FieldRef Name='String1' /><Value>TEST</Value></Eq>");
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
		[SuppressMessage("ReSharper", "EqualExpressionComparison")]
		public void SupportOperandOrder()
		{
			Test(n => n.Int1 == 1 && 2 == n.Int2 && 3 == 3,
				"<And><Eq><FieldRef Name='Int1' /><Value>1</Value></Eq><Eq><FieldRef Name='Int2' /><Value>2</Value></Eq></And>");
		}

		[TestMethod]
		public void ThrowIfInvalid()
		{
			CustomAssert.Throw<NotSupportedException>(() => Test(n => n.String1.Contains(n.String2), "SHOULD THROW"));
			CustomAssert.Throw<NotSupportedException>(() => Test(n => n.Bool1 == n.Bool2, "SHOULD THROW"));
		}

		public void Test(Expression<Func<VisitorsTestClass, bool>> original, string exprected)
		{
			var processor = new CamlPredicateProcessor();

			Assert.AreEqual(exprected, processor.Process(original).ToString());
		}
	}
}