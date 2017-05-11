using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Data.Translators.Predicate
{
	[TestClass]
	[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
	public class PushNotDownVisitorTest : BaseExpressionVisitorTest
	{
		[TestMethod]
		public void Visit_KeepsSame()
		{
			Given(obj => obj.Bool1).Expected(obj => obj.Bool1);

			Given(obj => obj.Bool1 && obj.Bool2).Expected(obj => obj.Bool1 && obj.Bool2);

			Given(obj => obj.Bool1 && obj.Int1 > 1).Expected(obj => obj.Bool1 && obj.Int1 > 1);
		}

		[TestMethod]
		public void Visit_Pushes_WhenOneNot()
		{
			Given(obj => !obj.Bool1).Expected(obj => !obj.Bool1);

			Given(obj => !(obj.Bool1 && obj.Bool2)).Expected(obj => !obj.Bool1 || !obj.Bool2);

			Given(obj => !(obj.Bool1 && obj.Int1 > 1)).Expected(obj => !obj.Bool1 || obj.Int1 <= 1);
		}

		[TestMethod]
		public void Visit_Pushes_WhenMultipleNot()
		{
			Given(obj => !(obj.Bool1 && !obj.Bool2)).Expected(obj => !obj.Bool1 || obj.Bool2);

			Given(obj => !(obj.Bool1 && !(obj.Int1 > 1))).Expected(obj => !obj.Bool1 || obj.Int1 > 1);
		}

		[TestMethod]
		public void Visit_Pushes_WhenNotWithCall()
		{
			Given(obj => !(obj.Bool1 && obj.String1.Contains("TEST")))
				.Expected(obj => !obj.Bool1 || !obj.String1.Contains("TEST"));
		}

		[TestMethod]
		public void Visit_Pushes_WhenNotWithConstBool()
		{
			Given(obj => !(obj.Bool1 && true))
				.Expected(obj => !obj.Bool1 || false);
		}

		protected override ExpressionVisitor TestableVisitor => new PushNotDownVisitor();
	}
}
