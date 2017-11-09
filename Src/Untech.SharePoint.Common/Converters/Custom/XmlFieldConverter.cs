using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Converters.Custom
{
	/// <summary>
	/// Represents field converter that can convert XML to object and vice versa.
	/// </summary>
	[PublicAPI]
	[SpFieldConverter("_Xml_")]
	public sealed class XmlFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }

		/// <summary>
		/// Initializes current instance with the specified <see cref="MetaField"/>
		/// </summary>
		/// <param name="field"></param>
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull(nameof(field), field);

			Field = field;
		}

		/// <summary>
		/// Converts SP field value to <see cref="MetaField.MemberType"/>.
		/// </summary>
		/// <param name="value">SP value to convert.</param>
		/// <returns>Member value.</returns>
		public object FromSpValue(object value)
		{
			var stringValue = (string)value;
			if (string.IsNullOrEmpty(stringValue))
			{
				return null;
			}

			var serializer = new XmlSerializer(Field.MemberType);

			using (var reader = XmlReader.Create(new StringReader(stringValue)))
			{
				return serializer.Deserialize(reader);
			}
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP field value.
		/// </summary>
		/// <param name="value">Member value to convert.</param>
		/// <returns>SP field value.</returns>
		public object ToSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var sb = new StringBuilder();
			var serializer = new XmlSerializer(Field.MemberType);
			var settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			};
			var ns = new XmlSerializerNamespaces();
			//ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			//ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
			//xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" 
			using (var xmlWriter = XmlWriter.Create(sb, settings))
			{
				serializer.Serialize(xmlWriter, value, ns);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP Caml value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Caml value.</returns>
		public string ToCamlValue(object value)
		{
			return (string)ToSpValue(value);
		}
	}
}
