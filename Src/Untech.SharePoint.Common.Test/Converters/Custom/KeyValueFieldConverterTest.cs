using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.Custom;

namespace Untech.SharePoint.Common.Test.Converters.Custom
{
	[TestClass]
	public class KeyValueFieldConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertDictionary()
		{
			Given<Dictionary<string, string>>()
				.CanConvertFromSp(null, null)
				.CanConvertFromSp("SomeVar:value1; AnotherVar:value2 ;ThirdVar:3", new Dictionary<string, string>
				{
					{"SomeVar", "value1"},
					{"AnotherVar", "value2"},
					{"ThirdVar", "3"}
				}, new DictionaryComparer())
				.CanConvertFromSp("Key:Value;NoValue:;Last:End", new Dictionary<string, string>
				{
					{"Key", "Value"},
					{"NoValue", null},
					{"Last", "End"}
				}, new DictionaryComparer())
				.CanConvertToSp(null, null)
				.CanConvertToSp(new Dictionary<string, string>
				{
					{"Key", "Value"},
					{"NoValue", null},
					{"Last", "End"}
				}, "Key:Value;NoValue:;Last:End")
				.CanConvertToCaml(null, "")
				.CanConvertToCaml(new Dictionary<string, string>
				{
					{"Key", "Value"},
					{"NoValue", null},
					{"Last", "End"}
				}, "Key:Value;NoValue:;Last:End");
		}

		[TestMethod]
		public void CanConvertEnumerable()
		{
			Given<IEnumerable<KeyValuePair<string, string>>>();
		}

		[TestMethod]
		public void CannotConvertList()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<List<KeyValuePair<string, string>>>());
		}

		protected override IFieldConverter GetConverter()
		{
			return new KeyValueFieldConverter();
		}

		public class DictionaryComparer : EqualityComparer<Dictionary<string, string>>
		{
			public override bool Equals(Dictionary<string, string> x, Dictionary<string, string> y)
			{
				foreach (var xPair in x)
				{
					if (!y.ContainsKey(xPair.Key))
					{
						return false;
					}

					if (y[xPair.Key] != xPair.Value)
					{
						return false;
					}
				}

				return true;
			}

			public override int GetHashCode(Dictionary<string, string> obj)
			{
				if (obj == null) return 0;
				int hash = 0;
				foreach (var pair in obj)
				{
					hash ^= pair.Key.GetHashCode();
					hash ^= (pair.Value ?? "").GetHashCode();
				}
				return hash;
			}
		}
	}
}
