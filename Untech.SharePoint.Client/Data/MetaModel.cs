using System;
using System.Collections.Generic;
using System.Reflection;
using Untech.SharePoint.Client.Extensions;

namespace Untech.SharePoint.Client.Data
{
	public abstract class MetaModel
	{
		public abstract MetaList GetList(MemberInfo memberInfo);

		public abstract MetaList GetList(string listTitle, Type itemType);

		public abstract IEnumerable<MetaList> GetLists();

		public override string ToString()
		{
			return string.Format("( Lists=[ {0} ]; )", GetLists().JoinToString());
		}
	}
}