using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Server.Converters.BuiltIn;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Test.Converters;

namespace Untech.SharePoint.Server.Test.Converters.BuiltIn
{
	[TestClass]
	public class ContentTypeIdFieldConverterTest : BaseConverterTest
	{
		protected override IFieldConverter GetConverter()
		{
			return new ContentTypeIdConverter();
		}
	}
}
