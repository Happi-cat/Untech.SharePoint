using System;
using Newtonsoft.Json;
using Untech.SharePoint.Core.Data;
using Untech.SharePoint.Core.Data.Converters;

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
			[SPField("Property")]
			public string Property { get; set; }

			[SPField("Created")]
			public DateTime Created { get; set; }

			[SPField("JsonObject", CustomConverterType = typeof (JsonFieldConverter))]
			public JsonSerializableObject JsonObject { get; set; }
		}
	}
}