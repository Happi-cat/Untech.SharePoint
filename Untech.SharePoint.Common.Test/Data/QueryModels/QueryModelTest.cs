using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Test.Data.QueryModels
{
	[TestClass]
	public class QueryModelTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			var model = new QueryModel
			{
				RowLimit = 1,
				Where =
					WhereModel.And(
						WhereModel.And(new ComparisonModel(ComparisonOperator.Eq, new FieldRefModel(), 1),
							new ComparisonModel(ComparisonOperator.Neq, new FieldRefModel(), 2)),
						new ComparisonModel(ComparisonOperator.Eq, new FieldRefModel(), "TEST"))
			};
			Assert.AreNotEqual( model.ToString(), "");
		}
	}
}
