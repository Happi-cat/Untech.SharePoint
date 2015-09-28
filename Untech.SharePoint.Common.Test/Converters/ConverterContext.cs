using System.Collections.Generic;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Converters
{
	public class ConverterDataContext : ISpContext
	{
		[SpList]
		public ISpList<ConverterDataEntity> Test { get; set; }
	}

	[SpContentType]
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