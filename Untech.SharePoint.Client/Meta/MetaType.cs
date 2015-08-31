using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Meta.Collections;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.Meta
{
	public class MetaType
	{
		private readonly MetaList _list;
		private readonly Type _type;

		public MetaType(MetaList list, Type type, IEnumerable<IMetaDataMemberProvider> dataMemberProviders)
		{
			Guard.CheckNotNull("list", list);
			Guard.CheckNotNull("type", type);

			_list = list;
			_type = type;

			DataMembers = new DataMemberCollection(dataMemberProviders.Select(n => n.GetMetaDataMember(this)));
		}

		public MetaList List { get { return _list; } }

		public Type Type { get { return _type; } }

		public DataMemberCollection DataMembers { get; private set; }

		public override string ToString()
		{
			return string.Format("( Type='{0}'; Members=[ {1} ]; )", Type, DataMembers.JoinToString());
		}
	}
}

