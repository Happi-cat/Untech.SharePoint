using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Client.Data
{
	internal class DataMemberCollection : IReadOnlyCollection<MetaDataMember>
	{
		private readonly Dictionary<string, MetaDataMember> _members;

		public DataMemberCollection(IEnumerable<MetaDataMember> members)
		{
			_members = members.ToDictionary(n => n.Name);
		}
		public MetaDataMember GetMemberByName(string name)
		{
			return _members[name];
		}

		public bool TryGetMemberByName(string name, out MetaDataMember member)
		{
			return _members.TryGetValue(name, out member);
		}

		public IEnumerator<MetaDataMember> GetEnumerator()
		{
			return _members.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count { get { return _members.Values.Count; } }
	}
}
