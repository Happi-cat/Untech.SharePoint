using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Converters;
using Untech.SharePoint.Mappings.Annotation;

namespace Untech.SharePoint.Configuration
{
	[TestClass]
	public class ConfigBuilderTest
	{
		[TestMethod]
		public void BuildConfig_ReturnsNotNull()
		{
			Assert.IsNotNull(new ConfigBuilder().BuildConfig());
		}

		[TestMethod]
		public void RegisterMappings_MappingCanBeResolved_WhenBuilt()
		{
			var config = new ConfigBuilder()
				.RegisterMappings(n => n.Annotated<AnnotatedContextMappingTest.Ctx>())
				.BuildConfig();

			Assert.IsNotNull(config.Mappings.Resolve(typeof(AnnotatedContextMappingTest.Ctx)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RegisterMappings_ThrowArgumentNull_WhenActionIsNull()
		{
			var config = new ConfigBuilder()
				.RegisterMappings<AnnotatedContextMappingTest.Ctx>(null);
		}

		[TestMethod]
		public void RegisterConverters_ConverterCanBeResolved_WhenBuilt()
		{
			var config = new ConfigBuilder()
				.RegisterConverters(n => n.Add<BuiltInTestFieldConverter>())
				.BuildConfig();

			Assert.IsNotNull(config.FieldConverters.Resolve(typeof(BuiltInTestFieldConverter)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RegisterConverters_ThrowArgumentNull_WhenActionIsNull()
		{
			var config = new ConfigBuilder()
				.RegisterConverters(null);
		}
	}
}
