using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class SpListSchema : IReadOnlyCollection<Field>
	{
		private Dictionary<string, Field> _fields;

		public SpListSchema(IEnumerable<Field> fields)
		{
			_fields = fields.ToDictionary<string, Field>(n => n.InternalName);
		}

		public Field GetFieldByInternalName(string internalName)
		{
			return _fields[internalName];
		}

		public bool TryGetFieldByInternalName(string internalName, out Field field)
		{
			return _fields.TryGetValue(internalName, out field);
		}

		public int Count
		{
			get { return _fields.Values.Count; }
		}

		public IEnumerator<Field> GetEnumerator()
		{
			return _fields.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
