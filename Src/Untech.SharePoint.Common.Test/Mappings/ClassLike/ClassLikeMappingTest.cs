using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Mappings.ClassLike
{
	[TestClass]
	public class ClassLikeMappingTest
	{
		[TestMethod]
		public void GetMetaContext_ReturnsMetaContext()
		{
			var mapping = new Mappings().ClassLike(new SmallDataContextMap());

			Assert.IsNotNull(mapping.GetMetaContext());
		}
	}
}
