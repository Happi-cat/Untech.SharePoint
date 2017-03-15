using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[TestClass]
	public class GuidConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertGuid()
		{
			var guid = Guid.NewGuid();

			CreateConverterForFieldWithType<Guid>()
				.CanConvertFromSp(guid, guid)
				.CanConvertFromSp(guid.ToString("B"), guid)
				.CanConvertFromSp(guid.ToString("D"), guid)
				.CanConvertFromSp(null, Guid.Empty)
				.CanConvertToSp(guid, guid)
				.CanConvertToCaml(guid, guid.ToString("D"));
		}

		[TestMethod]
		public void CanConvertNullableGuid()
		{
			var guid = Guid.NewGuid();

			CreateConverterForFieldWithType<Guid?>()
				.CanConvertFromSp(guid, guid)
				.CanConvertFromSp(guid.ToString("B"), guid)
				.CanConvertFromSp(guid.ToString("D"), guid)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(guid, guid)
				.CanConvertToSp(null, null)
				.CanConvertToCaml(guid, guid.ToString("D"))
				.CanConvertToCaml(null, "");
		}

		protected override IFieldConverter GetConverter()
		{
			return new GuidFieldConverter();
		}
	}
}