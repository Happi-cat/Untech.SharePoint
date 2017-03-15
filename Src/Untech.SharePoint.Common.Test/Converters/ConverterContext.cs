﻿using System.Diagnostics.CodeAnalysis;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters
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