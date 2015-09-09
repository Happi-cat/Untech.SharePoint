using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Common.Factory
{
	public class KeyedFactory<TKey, TObject>
	{
		private Dictionary<TKey, Func<TObject>> _factoryMethods = new Dictionary<TKey, Func<TObject>>();

		public void Register(TKey key, Func<TObject> builder)
		{
			if (IsRegistered(key))
			{
				throw new ArgumentException("Already registered");
			}

			_factoryMethods.Add(key, builder);
		}

		public void Register<TConcreteObject>(TKey key)
			where TConcreteObject : TObject, new()
		{
			Register(key, () => new TConcreteObject());
		}

		public bool IsRegistered(TKey key)
		{
			return _factoryMethods.ContainsKey(key);
		}

		public TObject Create(TKey key)
		{
			return _factoryMethods[key]();
		}
	}
}
