using System;
using System.Collections.Concurrent;
using Untech.SharePoint.Client.Utility;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class MetaTypePool
	{
		private readonly ConcurrentDictionary<Type, MetaType> _metaTypes = new ConcurrentDictionary<Type, MetaType>();

		public static MetaTypePool Instance
		{
			get { return Singleton<MetaTypePool>.GetInstance(); }
		}

		public MetaType Get(Type type)
		{
			return _metaTypes.GetOrAdd(type, CreateType);
		}

		public MetaType Get<T>()
		{
			return Get(typeof (T));
		}

		private MetaType CreateType(Type type)
		{
			return new MetaType(type);
		}
	}
}