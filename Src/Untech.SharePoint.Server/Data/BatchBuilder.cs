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
			_xmlWriter.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			_xmlWriter.WriteRaw("<Batch OnError=\"Return\">");
		}

		public void NewItem(SPList list, IEnumerable<KeyValuePair<string, string>> fields)
		{
			_counter++;
			_xmlWriter.WriteRaw($"<Method ID=\"{_counter}\">");
			_xmlWriter.WriteRaw($"<SetList Scope=\"Request\">{list.ID:D}</SetList>");
			_xmlWriter.WriteRaw("<SetVar Name=\"ID\">New</SetVar>");
			_xmlWriter.WriteRaw("<GetVar Name=\"ID\"></GetVar>");
			_xmlWriter.WriteRaw("<SetVar Name=\"Cmd\">Save</SetVar>");
			
			fields.Where(n=> n.Key != "ID")
				.Each(WriteField);

			_xmlWriter.WriteRaw("</Method>");
		}

		public void UpdateItem(SPList list, IEnumerable<KeyValuePair<string, string>> fields)
		{
			_counter++;
			_xmlWriter.WriteRaw($"<Method ID=\"{_counter}\">");
			_xmlWriter.WriteRaw($"<SetList Scope=\"Request\">{list.ID:D}</SetList>");
			_xmlWriter.WriteRaw("<SetVar Name=\"Cmd\">Save</SetVar>");

			fields.Each(WriteField);

			_xmlWriter.WriteRaw("</Method>");
		}

		public void DeleteItem(SPList list, string id)
		{
			_counter++;
			_xmlWriter.WriteRaw($"<Method ID=\"{_counter}\">");
			_xmlWriter.WriteRaw($"<SetList Scope=\"Request\">{list.ID:D}</SetList>");
			_xmlWriter.WriteRaw("<SetVar Name=\"Cmd\">Delete</SetVar>");

			WriteField("ID", id);

			_xmlWriter.WriteRaw("</Method>");
		}

		public void End()
		{
			_xmlWriter.WriteRaw("</Batch>");
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
			if (fieldInternalName != "ID")
			{
				fieldInternalName = "urn:schemas-microsoft-com:office:office#" + fieldInternalName;
			}

			_xmlWriter.WriteRaw($"<SetVar Name=\"{fieldInternalName}\">");
			_xmlWriter.WriteString(value);
			_xmlWriter.WriteRaw("</SetVar>");
		}
	}
}