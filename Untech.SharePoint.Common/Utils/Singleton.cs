using System;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Utils
{
	/// <summary>
	/// Represents container for instance singleton
	/// </summary>
	/// <typeparam name="T">Type of singleton instance</typeparam>
	[PublicAPI]
	public static class Singleton<T> where T : new()
	{
		// ReSharper disable once StaticMemberInGenericType
		[NotNull] private static readonly object Sync = new object();
		[CanBeNull] private static T _object;

		/// <summary>
		/// Gets signleton instance
		/// </summary>
		/// <returns>Intance of type <typeparamref name="T"/></returns>
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

		/// <summary>
		/// Gets signleton instance
		/// </summary>
		/// <param name="initializer">Instance initializer, i.e. action that will be called on first object access.</param>
		/// <returns>Intance of type <typeparamref name="T"/></returns>
		/// <exception cref="ArgumentNullException"><paramref name="initializer"/> is null.</exception>
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