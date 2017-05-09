using System;
using Untech.SharePoint.CodeAnnotations;

namespace Untech.SharePoint.Utils
{
	/// <summary>
	/// Represents container for instance singleton
	/// </summary>
	/// <typeparam name="T">Type of singleton instance</typeparam>
	[PublicAPI]
	public static class Singleton<T> where T : class, new()
	{
		// ReSharper disable once StaticMemberInGenericType
		[NotNull]
		private static readonly object s_sync = new object();
		[CanBeNull]
		private static T s_object;

		/// <summary>
		/// Gets singleton instance
		/// </summary>
		/// <returns>Instance of type <typeparamref name="T"/></returns>
		public static T GetInstance()
		{
			if (ReferenceEquals(s_object, null))
			{
				lock (s_sync)
				{
					if (ReferenceEquals(s_object, null))
					{
						s_object = new T();
					}
				}
			}
			return s_object;
		}

		/// <summary>
		/// Gets singleton instance
		/// </summary>
		/// <param name="initializer">Instance initializer, i.e. action that will be called on first object access.</param>
		/// <returns>Instance of type <typeparamref name="T"/></returns>
		/// <exception cref="ArgumentNullException"><paramref name="initializer"/> is null.</exception>
		public static T GetInstance(Action<T> initializer)
		{
			Guard.CheckNotNull(nameof(initializer), initializer);

			if (ReferenceEquals(s_object, null))
			{
				lock (s_sync)
				{
					if (ReferenceEquals(s_object, null))
					{
						var obj = new T();

						initializer(obj);

						s_object = obj;
					}
				}
			}
			return s_object;
		}
	}
}