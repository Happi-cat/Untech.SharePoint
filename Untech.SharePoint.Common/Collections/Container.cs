using System.Collections.Generic;

namespace Untech.SharePoint.Common.Collections
{
	public class Container<TKey, TObject>
	{
		private readonly Dictionary<TKey, TObject> _registeredObjects = new Dictionary<TKey, TObject>();

		public void Register(TKey key, TObject obj)
		{
			Guard.CheckNotNull("key", key);
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
			throw new KeyNotFoundException(string.Format("Unable to resolve key {0}", key));
		}

		public bool IsRegistered(TKey key)
		{
			return _registeredObjects.ContainsKey(key);
		}
	}
}
