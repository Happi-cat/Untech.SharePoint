using System;

namespace Untech.SharePoint.Core.Utility
{
	public class Singleton<T>
	{
		private static readonly Lazy<T> LazyObject = new Lazy<T>();

		public static T GetInstance()
		{
			return LazyObject.Value;
		}

		public static T GetInstance(Action<T> initializer)
		{
			if (!LazyObject.IsValueCreated)
			{
				var obj = LazyObject.Value;

				initializer(obj);
			}

			return LazyObject.Value;
		}
	}
}