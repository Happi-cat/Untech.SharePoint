using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Converters.BuiltIn;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Converters;

namespace Untech.SharePoint.Client.Test.Converters.BuiltIn
{
	[TestClass]
	public class UrlFieldConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertString()
		{
			Given<string>()
				.CanConvertFromSp(null, null)
				.CanConvertFromSp(new FieldUrlValue {Url = "http://google.com", Description = "Google It!"}, "http://google.com")
				.CanConvertToSp(null, null)
				.CanConvertToSp("http://google.com", new FieldUrlValue {Url = "http://google.com"}, new FieldUrlValueComparer())
				.CanConvertToCaml(null, "")
				.CanConvertToCaml("http://google.com", "http://google.com");
		}

		[TestMethod]
		public void CanConvertUrlInfo()
		{
			Given<UrlInfo>()
				.CanConvertFromSp(null, null)
				.CanConvertFromSp(new FieldUrlValue { Url = "http://google.com", Description = "Google It!" }, new UrlInfo{ Url = "http://google.com", Description = "Google It!" }, new UrlInfoComparer())
				.CanConvertToSp(null, null)
				.CanConvertToSp(new UrlInfo { Url = "http://google.com", Description = "Google It!" }, new FieldUrlValue { Url = "http://google.com", Description = "Google It!" }, new FieldUrlValueComparer())
				.CanConvertToCaml(null, "")
                .CanConvertToCaml(new UrlInfo { Url = "http://google.com" }, "http://google.com")
				.CanConvertToCaml(new UrlInfo { Url = "http://google.com", Description = "Google It!" }, "http://google.com, Google It!");
		}

		protected override IFieldConverter GetConverter()
		{
			return new UrlFieldConverter();
		}

		public class FieldUrlValueComparer : EqualityComparer<FieldUrlValue>
		{
			public override bool Equals(FieldUrlValue x, FieldUrlValue y)
			{
				return x.Url == y.Url && x.Description == y.Description;

			}

			public override int GetHashCode(FieldUrlValue obj)
			{
				if (obj == null) return 0;
				var hash1 = (obj.Url ?? "").GetHashCode();
				var hash2 = (obj.Description ?? "").GetHashCode();
				return hash1 ^ hash2;
			}
		}

		public class UrlInfoComparer : EqualityComparer<UrlInfo>
		{
			public override bool Equals(UrlInfo x, UrlInfo y)
			{
				return x.Url == y.Url && x.Description == y.Description;

			}

			public override int GetHashCode(UrlInfo obj)
			{
				if (obj == null) return 0;
				var hash1 = (obj.Url ?? "").GetHashCode();
				var hash2 = (obj.Description ?? "").GetHashCode();
				return hash1 ^ hash2;
			}
		}
	}
}