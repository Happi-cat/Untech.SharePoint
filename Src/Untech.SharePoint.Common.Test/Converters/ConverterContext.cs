using System.Diagnostics.CodeAnalysis;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Configuration;
using Untech.SharePoint.Data;
using Untech.SharePoint.Mappings;
using Untech.SharePoint.Mappings.Annotation;
using Untech.SharePoint.MetaModels;

namespace Untech.SharePoint.Converters
{
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public enum ExampleEnum
	{
		Value1,
		Value2,
		Value3
	}

	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
	public class ConverterDataContext : ISpContext
	{
		[SpList("Test")]
		public ISpList<ConverterDataEntity> Test { get; set; }

		public Config Config { get; }
		public IMappingSource MappingSource { get; }
		public MetaContext Model { get; }
	}

	[SpContentType]
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public class ConverterDataEntity
	{
		[SpField]
		public string String { get; set; }

		[SpField]
		public int Int { get; set; }

		[SpField]
		public bool Bool { get; set; }
	}
}