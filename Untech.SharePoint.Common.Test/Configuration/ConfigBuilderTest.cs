using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Test.Mappings.Annotation.Models;

namespace Untech.SharePoint.Common.Test.Configuration
{
	[TestClass]
	public class ConfigBuilderTest
	{
		[TestMethod]
		public void CanBuildConfig()
		{
			Assert.IsNotNull(new ConfigBuilder().BuildConfig());
		}

		[TestMethod]
		public void CanRegisterMappings()
		{
			var config = new ConfigBuilder()
				.RegisterMappings(n => n.Annotated<AnnotatedContext>())
				.BuildConfig();

			Assert.IsNotNull(config.Mappings.Resolve<AnnotatedContext>());
		}

		[TestMethod]
		public void CanRegisterBuiltInConverters()
		{
			var config = new ConfigBuilder()
				.RegisterConverters(n => n.AddFromAssembly(GetType().Assembly))
				.BuildConfig();

			Assert.IsNotNull(config.FieldConverters.Resolve("BUILT_IN_TEST_CONVERTER"));
		}

		[TestMethod]
		public void CanRegisterConverter()
		{
			var config = new ConfigBuilder()
				.RegisterConverters(n => n.Add<BuiltInFieldConverter>())
				.BuildConfig();

			Assert.IsNotNull(config.FieldConverters.Resolve(typeof(BuiltInFieldConverter)));
		}

		[TestMethod]
		public void CanChainCalls()
		{
			var config = new ConfigBuilder()
				.RegisterMappings(n => n.Annotated<AnnotatedContext>())
				.RegisterConverters(n => n.AddFromAssembly(GetType().Assembly))
				.RegisterConverters(n => n.Add<BuiltInFieldConverter>())
				.BuildConfig();

			Assert.IsNotNull(config.Mappings.Resolve<AnnotatedContext>());
			Assert.IsNotNull(config.FieldConverters.Resolve("BUILT_IN_TEST_CONVERTER"));
			Assert.IsNotNull(config.FieldConverters.Resolve(typeof(BuiltInFieldConverter)));
		}
	}
}
