using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Configuration;

namespace Untech.SharePoint.Common.Test.Mappings.ClassLike
{

	[TestClass]
	public class ClassLikeMappingTest
	{
		[TestMethod]
		public void CanRun()
		{
			var config = new ConfigBuilder()
				.RegisterMappings(n => n.ClassLike(new SmallDataContextMap()))
				.BuildConfig();

			var ctxModel = config.Mappings.Resolve(typeof (SmallDataContext)).GetMetaContext();
		}
	}
}
