using System;

namespace Untech.SharePoint.Common.Collections
{
	public class KeyedFactory<TKey, TObject> : Container<TKey, Func<TObject>>
	{
		public void Register<TConcreteObject>(TKey key)
			where TConcreteObject : TObject, new()
		{
			base.Register(key, () => new TConcreteObject());
		}

		public TObject Create(TKey key)
		{
			return Resolve(key)();
		}
	}
}
