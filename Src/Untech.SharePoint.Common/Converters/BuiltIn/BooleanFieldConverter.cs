using System;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Converters.BuiltIn
{
	[SpFieldConverter("Boolean")]
	[UsedImplicitly]
	internal class BooleanFieldConverter : IFieldConverter
	{
		private bool _isNullableMemberType;

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull(nameof(field), field);

			var memberType = field.MemberType;
			if (memberType.IsNullable())
			{
				_isNullableMemberType = true;
				memberType = memberType.GetGenericArguments()[0];
			}
			if (memberType != typeof(bool))
			{
				throw new ArgumentException("Member type should be bool or Syste.Nullable<bool>.");
			}
		}

		public object FromSpValue(object value)
		{
			if (_isNullableMemberType)
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