using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Server.Data
{
	internal class BatchBuilder
	{
		private readonly StringBuilder _sb;
		private readonly StringWriter _sw;
		private int _counter;

		public BatchBuilder()
		{
			_counter = 0;
			_sb = new StringBuilder();
			_sw = new StringWriter(_sb);
		}

		public void Begin()
		{
			_sw.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			_sw.Write("<Batch OnError=\"Return\">");
		}

		public void NewItem(SPList list, IEnumerable<KeyValuePair<string, string>> fields)
		{
			_counter++;
			_sw.Write("<Method ID=\"{0}\">", _counter);
			_sw.Write("<SetList Scope=\"Request\">{0:D}</SetList>", list.ID);
			_sw.Write("<SetVar Name=\"ID\">{0}</SetVar>", "New");
			_sw.Write("<GetVar Name=\"ID\"></GetVar>");
			_sw.Write("<SetVar Name=\"Cmd\">Save</SetVar>");
			
			fields.Where(n=> n.Key != "ID")
				.Each(WriteField);

			_sw.Write("</Method>");
		}

		public void UpdateItem(SPList list, IEnumerable<KeyValuePair<string, string>> fields)
		{
			_counter++;
			_sw.Write("<Method ID=\"{0}\">", _counter);
			_sw.Write("<SetList Scope=\"Request\">{0:D}</SetList>", list.ID);
			_sw.Write("<SetVar Name=\"Cmd\">Save</SetVar>");

			fields.Each(WriteField);

			_sw.Write("</Method>");
		}

		public void DeleteItem(SPList list, string id)
		{
			_counter++;
			_sw.Write("<Method ID=\"{0}\">", _counter);
			_sw.Write("<SetList Scope=\"Request\">{0:D}</SetList>", list.ID);
			_sw.Write("<SetVar Name=\"Cmd\">Delete</SetVar>");

			WriteField("ID", id);

			_sw.Write("</Method>");
		}

		public void End()
		{
			_sw.Write("</Batch>");
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

			_sw.Write("<SetVar Name=\"{0}\">", fieldInternalName);
			_sw.Write(SecurityElement.Escape(value));
			_sw.Write("</SetVar>");
		}
	}
}