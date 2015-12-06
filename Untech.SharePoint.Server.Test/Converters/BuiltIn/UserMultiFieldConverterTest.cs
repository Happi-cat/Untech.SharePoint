using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Server.Converters.BuiltIn;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Converters;

namespace Untech.SharePoint.Server.Test.Converters.BuiltIn
{
	[TestClass]
	public class UserMultiFieldConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanSupportArray()
		{
			Given<UserInfo[]>();
		}

		[TestMethod]
		public void CanSupportTypesAssignableFromList()
		{
			Given<IEnumerable<UserInfo>>();
			Given<ICollection<UserInfo>>();
			Given<IReadOnlyCollection<UserInfo>>();
			Given<IList<UserInfo>>();
			Given<List<UserInfo>>();
		}

		protected override IFieldConverter GetConverter()
		{
			return new UserMultiFieldConverter();
		}
	}
}