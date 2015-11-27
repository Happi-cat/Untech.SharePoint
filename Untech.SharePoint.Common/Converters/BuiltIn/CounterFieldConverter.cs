using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Counter")]
	[UsedImplicitly]
	internal class CounterFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;

			if (field.MemberType != typeof (int))
			{
				throw new ArgumentException("Only Int32 member type allowed");
			}
		}

		public object FromSpValue(object value)
		{
			return (int)value;
		}

		public object ToSpValue(object value)
		{
			return value;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}
	}
}