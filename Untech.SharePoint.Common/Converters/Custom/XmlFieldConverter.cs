using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.Custom
{
	public class XmlFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			var serializer = new DataContractSerializer(Field.MemberType);
			
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes((string) value)))
			{
				return serializer.ReadObject(stream);
			}
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			var serializer = new DataContractSerializer(Field.MemberType);
			var sb = new StringBuilder();

			using (var textWriter = new StringWriter(sb))
			using (var xmlWriter = new XmlTextWriter(textWriter))
			{
				serializer.WriteObject(xmlWriter, value);
			}

			return sb.ToString();
		}

		public string ToCamlValue(object value)
		{
			return (string) ToSpValue(value);
		}
	}
}
