using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Text")]
	[SpFieldConverter("Note")]
	[SpFieldConverter("Choice")]
	[UsedImplicitly]
	internal class TextFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			return (string) value;
		}

		public object ToSpValue(object value)
		{
			return (string) value;
		}

		public string ToCamlValue(object value)
		{
			return (string) ToSpValue(value);
		}
	}
}
