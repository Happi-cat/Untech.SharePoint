using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Guid")]
	[UsedImplicitly]
	internal class GuidFieldConverter : MultiTypeFieldConverter
	{
		public override void Initialize(MetaField field)
		{
			base.Initialize(field);
			if (field.MemberType == typeof(Guid))
			{
				Internal = new GuidFieldConverter();
			}
			else if (field.MemberType == typeof(Guid?))
			{
				Internal = new NullableGuidTypeConverter();
			}
			else
			{
				throw new ArgumentException("MemberType is invalid");
			}
		}

		private class GuidTypeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{

			}

			public object FromSpValue(object value)
			{
				return value != null ? new Guid(value.ToString()) : Guid.Empty;
			}

			public object ToSpValue(object value)
			{
				var guidValue = (Guid)value;
				return guidValue != Guid.Empty ? guidValue : (object)null;
			}

			public string ToCamlValue(object value)
			{
				return Convert.ToString(ToSpValue(value));
			}
		}

		private class NullableGuidTypeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{

			}

			public object FromSpValue(object value)
			{
				return value != null ? new Guid(value.ToString()) : (Guid?)null;
			}

			public object ToSpValue(object value)
			{
				return (Guid?) value;
			}

			public string ToCamlValue(object value)
			{
				return Convert.ToString(ToSpValue(value));
			}
		}
	}
}