using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Meta.Collections
{
	public sealed class SpFieldCollection : IReadOnlyCollection<Field>
	{
		private readonly ReadOnlyDictionary<string, Field> _fields;

		public SpFieldCollection(IEnumerable<Field> fields)
		{
			_fields = new ReadOnlyDictionary<string,Field>(fields.ToDictionary(n => n.InternalName));
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
