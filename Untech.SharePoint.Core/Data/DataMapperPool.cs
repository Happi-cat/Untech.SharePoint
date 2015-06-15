using System;
using System.Collections.Concurrent;
using Untech.SharePoint.Core.Utility;

namespace Untech.SharePoint.Core.Data
{
	internal class DataMapperPool
	{
		private readonly ConcurrentDictionary<Type, DataMapper> _cachedMappers = new ConcurrentDictionary<Type, DataMapper>();

		public static DataMapperPool Instance
		{
			get { return Singleton<DataMapperPool>.GetInstance(); }
		}

		public DataMapper Get(Type type)
		{
			return _cachedMappers.GetOrAdd(type, InstantiateMapper);
		}

		public DataMapper Get<T>()
		{
			return Get(typeof (T));
		}

		private static DataMapper InstantiateMapper(Type type)
		{
			var mapper = new DataMapper();

			mapper.Initialize(type);

			return mapper;
		}
	}
}