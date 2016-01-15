using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Server.Data
{
	internal class BatchBuilder
	{
		private readonly StringBuilder _sb;
		private readonly XmlTextWriter _xmlWriter;
		private int _counter;

		public BatchBuilder()
		{
			_counter = 0;
			_sb = new StringBuilder();
			_xmlWriter = new XmlTextWriter(new StringWriter(_sb));
		}

		public void Begin()
		{
			_xmlWriter.WriteStartElement("Batch");
		}

		public int NewItem(SPList list, IEnumerable<KeyValuePair<string, string>> fields)
		{
			_counter++;
			_xmlWriter.WriteStartElement("Method");
			_xmlWriter.WriteAttributeString("ID", _counter.ToString());
			_xmlWriter.WriteAttributeString("Cmd", "New");

			_xmlWriter.WriteElementString("SetList", list.ID.ToString("D"));

			fields.Where( n=> n.Key != "ID")
				.Each(WriteField);

			_xmlWriter.WriteEndElement();
			return _counter;
		}

		public int UpdateItem(SPList list, IEnumerable<KeyValuePair<string, string>> fields)
		{
			_counter++;
			_xmlWriter.WriteStartElement("Method");
			_xmlWriter.WriteAttributeString("ID", _counter.ToString());
			_xmlWriter.WriteAttributeString("Cmd", "Update");

			_xmlWriter.WriteElementString("SetList", list.ID.ToString("D"));

			fields.Each(WriteField);

			_xmlWriter.WriteEndElement();
			return _counter;
		}

		public int DeleteItem(SPList list, string id)
		{
			_counter++;
			_xmlWriter.WriteStartElement("Method");
			_xmlWriter.WriteAttributeString("ID", _counter.ToString());
			_xmlWriter.WriteAttributeString("Cmd", "Delete");

			_xmlWriter.WriteElementString("SetList", list.ID.ToString("D"));

			WriteField("ID", id);

			_xmlWriter.WriteEndElement();
			return _counter;
		}

		public void End()
		{
			_xmlWriter.WriteEndElement();

		}

		public override string ToString()
		{
			return _sb.ToString();
		}

		private void WriteField(KeyValuePair<string, string> pair)
		{
			WriteField(pair.Key, pair.Value);
		}

		private void WriteField(string fieldInternalName, string value)
		{
			_xmlWriter.WriteStartElement("SetVar");
			_xmlWriter.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:office#" + fieldInternalName);
			_xmlWriter.WriteString(value);
			_xmlWriter.WriteEndElement();
		}
	}
}