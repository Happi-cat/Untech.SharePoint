using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	public sealed class MetaFieldCollection : IReadOnlyCollection<MetaField>
	{
		private readonly ReadOnlyDictionary<string, MetaField> _fields;

		public MetaFieldCollection(IEnumerable<MetaField> fields)
		{
			_fields = new ReadOnlyDictionary<string, MetaField>(fields.ToDictionary(n => n.MemberName));
		}

		public IEnumerator<MetaField> GetEnumerator()
		{
			return _fields.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count { get { return _fields.Values.Count; } }

		public MetaField GetByMemberName(string memberName)
		{
			return _fields[memberName];
		}
	}
}