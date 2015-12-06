using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Converters.BuiltIn;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Converters;

namespace Untech.SharePoint.Client.Test.Converters.BuiltIn
{
	[TestClass]
	public class LookupMultiFieldConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanSupportArray()
		{
			Given<ObjectReference[]>();
		}

		[TestMethod]
		public void CanSupportTypesAssignableFromList()
		{
			Given<IEnumerable<ObjectReference>>();
			Given<ICollection<ObjectReference>>();
			Given<IReadOnlyCollection<ObjectReference>>();
			Given<IList<ObjectReference>>();
			Given<List<ObjectReference>>();
		}

		protected override IFieldConverter GetConverter()
		{
			return new LookupMultiFieldConverter();
		}
	}
}