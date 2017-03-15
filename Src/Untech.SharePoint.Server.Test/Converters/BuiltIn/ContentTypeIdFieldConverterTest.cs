using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;

namespace Untech.SharePoint.Server.Converters.BuiltIn
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
