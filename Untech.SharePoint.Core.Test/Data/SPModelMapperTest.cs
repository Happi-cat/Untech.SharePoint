using System;
using Newtonsoft.Json;
using Untech.SharePoint.Core.Data;
using Untech.SharePoint.Core.Data.Converters;
using Untech.SharePoint.Core.Data.Converters.Custom;

namespace Untech.SharePoint.Core.Test.Data
{
	public class SPModelMapperTest
	{
		public class JsonSerializableObject
		{
			[JsonProperty]
			public string Property1 { get; set; }
		}

		public class TestClass
		{
			[SpField("Property")]
			public string Property { get; set; }

			[SpField("Created")]
			public DateTime Created { get; set; }

			[SpField("JsonObject", CustomConverterType = typeof (JsonFieldConverter))]
			public JsonSerializableObject JsonObject { get; set; }
		}
	}
}