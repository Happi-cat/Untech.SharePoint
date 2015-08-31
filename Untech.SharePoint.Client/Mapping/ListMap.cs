using System;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Client.Meta;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.Mapping
{
	public class ListMap : IMetaListProvider
	{
		private IMetaTypeProvider _type;

		private string _listTitle;

		public ListMap(string name)
		{
			_listTitle = name;
		}

		public ListMap ContentType<TProvider>()
			where TProvider: IMetaTypeProvider, new ()
		{
			return ContentType(new TProvider());
		}

		public ListMap ContentType(IMetaTypeProvider provider)
		{
			_type = provider;
			return this;
		}

		public MetaList GetMetaList()
		{
			return new MetaList(_listTitle, null, _type);
		}
	}
}