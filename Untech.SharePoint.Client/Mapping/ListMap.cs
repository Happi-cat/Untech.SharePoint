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

	internal class TestModel
	{
		public string Property1 { get; set; }

		public string Property2 { get; set; }
	}

	public class ListInstancemap : ListMap
	{
		public ListInstancemap()
			: base("title")
		{
			ContentType(CreateCt());

		}

		public IMetaTypeProvider CreateCt()
		{
			var ctMap = new TypeMap<TestModel>();

			ctMap.Map(x => x.Property1).InternalName("name").TypeAsString("URL");
			ctMap.Map(x => x.Property2);

		}
	}
}