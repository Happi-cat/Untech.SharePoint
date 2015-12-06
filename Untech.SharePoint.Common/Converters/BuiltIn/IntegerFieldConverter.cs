using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Integer")]
	[SpFieldConverter("Counter")]
	[UsedImplicitly]
	internal class IntegerFieldConverter : IFieldConverter
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
			if (memberType != typeof(int))
			{
				throw new ArgumentException("Invalid");
			}
		}

		public object FromSpValue(object value)
		{
			if (IsNullableMemberType)
				return (int?)value;

			return (int?)value ?? 0;
		}

		public object ToSpValue(object value)
		{
			return (int?)value;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}
	}
}