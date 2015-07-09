using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.SharePoint;
using Newtonsoft.Json;
using Untech.SharePoint.Core.Data;
using Untech.SharePoint.Core.Data.Converters.Custom;
using Untech.SharePoint.Core.Extensions;

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
			[SpField(InternalName = "Property")]
			public string Property { get; set; }

			[SpField(InternalName = "Created")]
			public DateTime Created { get; set; }

			[SpField(InternalName = "JsonObject", CustomConverterType = typeof(JsonFieldConverter))]
			public JsonSerializableObject JsonObject { get; set; }
		}


		private IEnumerable<TestClass> Do(SPList list)
		{
			return list.AsQueryable<TestClass>()
				.Where(n => n.Created > DateTime.Now)
				.Where(n => n.Property.Contains("a"))
				.Take(10)
				.ToList();
		}
	}
}