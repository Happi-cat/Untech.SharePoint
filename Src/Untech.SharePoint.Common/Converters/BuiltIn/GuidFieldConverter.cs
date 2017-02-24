using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Guid")]
	[UsedImplicitly]
	internal class GuidFieldConverter : MultiTypeFieldConverter
	{
		private static readonly IReadOnlyDictionary<Type, Func<IFieldConverter>> s_typeConverters = new Dictionary<Type, Func<IFieldConverter>>
		{
			{typeof (Guid), () => new GuidTypeConverter()},
			{typeof (Guid?), () => new NullableGuidTypeConverter()}
		};

		public override void Initialize(MetaField field)
		{
			base.Initialize(field);
			if (s_typeConverters.ContainsKey(field.MemberType))
			{
				Internal = s_typeConverters[field.MemberType]();
			}
			else
			{
				throw new ArgumentException("Member type should be System.Guid or System.Nullable<System.Guid>.");
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
				return (Guid)value;
			}

			public string ToCamlValue(object value)
			{
				var guidValue = (Guid)value;
				return guidValue.ToString("D");
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
				return (Guid?)value;
			}

			public string ToCamlValue(object value)
			{
				var guidValue = (Guid?)value;
				return guidValue?.ToString("D") ?? "";
			}
		}
	}
}