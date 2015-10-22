using System.Collections.Generic;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Collections
{
	public class Container<TKey, TObject> : IEnumerable<KeyValuePair<TKey, TObject>>
	{
		private readonly Dictionary<TKey, TObject> _registeredObjects = new Dictionary<TKey, TObject>();

		public void Register(TKey key, TObject obj)
		{
			Guard.CheckNotNull("obj", obj);

			if (IsRegistered(key))
			{
				_registeredObjects[key] = obj;
			}
			else
			{
				_registeredObjects.Add(key, obj);
			}
		}

		public TObject Resolve(TKey key)
		{
			if (IsRegistered(key))
			{
				return _registeredObjects[key];
			}
			throw Error.KeyNotFound(key);
		}

		public bool IsRegistered(TKey key)
		{
			Guard.CheckNotNull("key", key);

			return _registeredObjects.ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<TKey, TObject>> GetEnumerator()
		{
			return _registeredObjects.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
