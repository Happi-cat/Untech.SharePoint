using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Common.Collections
{
	public class Container<TKey, TObject>
	{
		public Dictionary<TKey, TObject> _registeredObjects = new Dictionary<TKey, TObject>();

		public void Register(TKey key, TObject obj)
		{
			_registeredObjects.Add(key, obj);
		}

		public TObject Resolve(TKey key)
		{
			return _registeredObjects[key];
		}

		public bool IsRegistered(TKey key)
		{
			return _registeredObjects.ContainsKey(key);
		}
	}
}
