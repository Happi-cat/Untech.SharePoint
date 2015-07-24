using System;

namespace Untech.SharePoint.Core.Utility
{
	public class Singleton<T> where T: new()
	{
		private static readonly object Sync = new object();
		private static T _object;

		public static T GetInstance()
		{
			if (_object == null)
			{
				lock (Sync)
				{
					if (_object == null)
					{
						_object = new T();
					}
				}
			}
			return _object;
		}

		public static T GetInstance(Action<T> initializer)
		{
			Guard.ThrowIfArgumentNull(initializer, "initializer");

			if (_object == null)
			{
				lock (Sync)
				{
					if (_object == null)
					{
						_object = new T();

						initializer(_object);
					}
				}
			}
			return _object;
		}
	}
}