using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Converters;

namespace Untech.SharePoint.Client.Converters.BuiltIn
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
