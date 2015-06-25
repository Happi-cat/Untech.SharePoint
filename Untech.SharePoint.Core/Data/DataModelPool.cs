using System;
using System.Collections.Concurrent;
using Untech.SharePoint.Core.Utility;

namespace Untech.SharePoint.Core.Data
{
	internal class DataModelPool
	{
		private readonly ConcurrentDictionary<Type, DataModel> _cachedMappers = new ConcurrentDictionary<Type, DataModel>();

		public static DataModelPool Instance
		{
			get { return Singleton<DataModelPool>.GetInstance(); }
		}

		public DataModel Get(Type type)
		{
			return _cachedMappers.GetOrAdd(type, InstantiateModel);
		}

		public DataModel Get<T>()
		{
			return Get(typeof (T));
		}

		private static DataModel InstantiateModel(Type type)
		{
			var mapper = new DataModel();

			mapper.Initialize(type);

			return mapper;
		}
	}
}