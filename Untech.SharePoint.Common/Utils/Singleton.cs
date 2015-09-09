using System;

namespace Untech.SharePoint.Common.Utils
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
			Guard.CheckNotNull("initializer", initializer);

			if (_object == null)
			{
				lock (Sync)
				{
					if (_object == null)
					{
						var obj = new T();

						initializer(obj);

						_object = obj;
					}
				}
			}
			return _object;
		}
	}
}