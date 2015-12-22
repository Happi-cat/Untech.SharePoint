using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Boolean")]
	[UsedImplicitly]
	internal class BooleanFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }
		private bool IsNullableMemberType { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
			var memberType = field.MemberType;
			if (memberType.IsNullable())
			{
				IsNullableMemberType = true;
				memberType = memberType.GetGenericArguments()[0];
			}
			if (memberType != typeof(bool))
			{
				throw new ArgumentException("Member type should be bool or Syste.Nullable<bool>.");
			}
		}

		public object FromSpValue(object value)
		{
			if (IsNullableMemberType)
				return (bool?)value;

			return (bool?)value ?? false;
		}

		public object ToSpValue(object value)
		{
			return (bool?)value;
		}

		public string ToCamlValue(object value)
		{
			var boolValue = (bool?)value;
			if (boolValue.HasValue)
			{
				return boolValue.Value ? "1" : "0";
			}
			return "";
		}
	}
}