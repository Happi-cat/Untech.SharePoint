using System;
using System.Collections.Concurrent;
using Untech.SharePoint.Client.Utility;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class MetaModelPool
	{
		private readonly ConcurrentDictionary<Type, MetaModel> _cachedMappers = new ConcurrentDictionary<Type, MetaModel>();

		public static MetaModelPool Instance
		{
			get { return Singleton<MetaModelPool>.GetInstance(); }
		}

		public MetaModel Get(Type type)
		{
			return _cachedMappers.GetOrAdd(type, CreateModel);
		}

		public MetaModel Get<T>()
		{
			return Get(typeof (T));
		}

		private static MetaModel CreateModel(Type type)
		{
			var factory = new MetaModelFactory(type);

			return factory.Get();
		}
	}
}