using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.ExpressionVisitors;

namespace Untech.SharePoint.Common.Test.Data.Translators.ExpressionVisitors
{
	[TestClass]
	public class PushNotDownVisitorTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void CanRemainSame()
		{
			Test(obj => obj.Bool1, obj => obj.Bool1);
			Test(obj => obj.Bool1 && obj.Bool2, obj => obj.Bool1 && obj.Bool2);
			Test(obj => obj.Bool1 && obj.Int1 > 1, obj => obj.Bool1 && obj.Int1 > 1);
		}

		[TestMethod]
		public void CanNegateOneNot()
		{
			Test(obj => !obj.Bool1, obj => !obj.Bool1);
			Test(obj => !(obj.Bool1 && obj.Bool2), obj => !obj.Bool1 || !obj.Bool2);
			Test(obj => !(obj.Bool1 && obj.Int1 > 1), obj => !obj.Bool1 || obj.Int1 <= 1);
		}


		[TestMethod]
		public void CanNegateMultipleNot()
		{
			Test(obj => !(obj.Bool1 && !obj.Bool2), obj => !obj.Bool1 || obj.Bool2);
			Test(obj => !(obj.Bool1 && !(obj.Int1 > 1)), obj => !obj.Bool1 || obj.Int1 > 1);
		}

		[TestMethod]
		public void CanNegateWithCustomConditions()
		{
			Test(obj => !(obj.Bool1 && obj.String1.Contains("TEST")), obj => !obj.Bool1 || !obj.String1.Contains("TEST"));
		}

		[TestMethod]
		public void CanNegateBoolConsts()
		{
			Test(obj => !(obj.Bool1 && true), obj => !obj.Bool1 || false);
		}

		protected override ExpressionVisitor Visitor
		{
			get { return new PushNotDownVisitor(); }
		}
	}
}
