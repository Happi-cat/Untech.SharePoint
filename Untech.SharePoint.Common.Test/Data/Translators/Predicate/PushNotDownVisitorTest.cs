using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.Predicate;

namespace Untech.SharePoint.Common.Test.Data.Translators.Predicate
{
	[TestClass]
	[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
	public class PushNotDownVisitorTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void CanRemainSame()
		{
			Given(obj => obj.Bool1).Expected(obj => obj.Bool1);

			Given(obj => obj.Bool1 && obj.Bool2).Expected(obj => obj.Bool1 && obj.Bool2);

			Given(obj => obj.Bool1 && obj.Int1 > 1).Expected(obj => obj.Bool1 && obj.Int1 > 1);
		}

		[TestMethod]
		public void CanPushOneNot()
		{
			Given(obj => !obj.Bool1).Expected(obj => !obj.Bool1); 

			Given(obj => !(obj.Bool1 && obj.Bool2)).Expected(obj => !obj.Bool1 || !obj.Bool2);

			Given(obj => !(obj.Bool1 && obj.Int1 > 1)).Expected(obj => !obj.Bool1 || obj.Int1 <= 1);
		}


		[TestMethod]
		public void CanPushMultipleNot()
		{
			Given(obj => !(obj.Bool1 && !obj.Bool2)).Expected(obj => !obj.Bool1 || obj.Bool2);

			Given(obj => !(obj.Bool1 && !(obj.Int1 > 1))).Expected(obj => !obj.Bool1 || obj.Int1 > 1);
		}

		[TestMethod]
		public void CanPushNotWithCall()
		{
			Given(obj => !(obj.Bool1 && obj.String1.Contains("TEST")))
				.Expected(obj => !obj.Bool1 || !obj.String1.Contains("TEST"));
		}

		[TestMethod]
		public void CanPushNotWithBoolConst()
		{
			Given(obj => !(obj.Bool1 && true))
				.Expected(obj => !obj.Bool1 || false);
		}

		protected override ExpressionVisitor Visitor
		{
			get { return new PushNotDownVisitor(); }
		}
	}
}
