using System;
using System.IO;
using Microsoft.SharePoint;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Untech.SharePoint.Core.Data.Fields.Converters
{
	public class XmlFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			var serializer = new DataContractSerializer(propertyType);
			
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes((string) value ?? "")))
			{
				return serializer.ReadObject(stream);
			}
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			var serializer = new DataContractSerializer(propertyType);
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
