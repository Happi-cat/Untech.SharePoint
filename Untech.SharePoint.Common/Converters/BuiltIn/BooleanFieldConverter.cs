using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Boolean")]
	internal class BooleanFieldConverter : MultiTypeFieldConverter
	{
		public override void Initialize(MetaField field)
		{
			base.Initialize(field);
			if (field.MemberType == typeof (bool))
			{
				Internal = new BoolTypeConverter();
			}
			else if (field.MemberType == typeof (bool?))
			{
				Internal = new NullableBoolTypeConverter();
			}
			else
			{
				throw new ArgumentException("MemberType is invalid");
			}
		}

		public class BoolTypeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
				
			}

			public object FromSpValue(object value)
			{
				return (bool) value;
			}

			public object ToSpValue(object value)
			{
				return (bool) value;
			}

			public string ToCamlValue(object value)
			{
				return (bool) value ? "1" : "0";
			}
		}

		public class NullableBoolTypeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{

			}

			public object FromSpValue(object value)
			{
				return (bool?)value;
			}

			public object ToSpValue(object value)
			{
				return (bool?)value;
			}

			public string ToCamlValue(object value)
			{
				var boolValue = (bool?)value ;
				if (boolValue.HasValue)
				{
					return boolValue.Value ? "1" : "0";
				}
				return "";
			}
		}
	}
}