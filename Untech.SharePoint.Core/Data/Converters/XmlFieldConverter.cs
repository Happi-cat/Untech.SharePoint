using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	public class XmlFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(string))
				throw new ArgumentException("SPField with string value type only supported");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			if (value == null) return null;

			var serializer = new DataContractSerializer(PropertyType);
			
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes((string) value ?? "")))
			{
				return serializer.ReadObject(stream);
			}
		}

		public object ToSpValue(object value)
		{
			if (value == null) return null;

			var serializer = new DataContractSerializer(PropertyType);
			var sb = new StringBuilder();

			using (var textWriter = new StringWriter(sb))
			using (var xmlWriter = new XmlTextWriter(textWriter))
			{
				serializer.WriteObject(xmlWriter, value);
			}

			return sb.ToString();
		}
	}
}
