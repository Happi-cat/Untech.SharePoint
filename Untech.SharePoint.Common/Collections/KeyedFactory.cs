using System;
using JetBrains.Annotations;

namespace Untech.SharePoint.Common.Collections
{
	/// <summary>
	/// Represents a keyed objects factory.
	/// </summary>
	/// <typeparam name="TKey">The key of the object to create.</typeparam>
	/// <typeparam name="TObject">The base type of objects to create.</typeparam>
	public class KeyedFactory<TKey, TObject> : Container<TKey, Func<TObject>>
	{
		/// <summary>
		/// Gets or adds object creator asscoaited with the specified key. 
		/// </summary>
		/// <typeparam name="TConcreteObject">Type of object to create. This type should have public default constructor.</typeparam>
		/// <param name="key">The key of the object creator.</param>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
		public void Register<TConcreteObject>([NotNull]TKey key)
			where TConcreteObject : TObject, new()
		{
			base.Register(key, () => new TConcreteObject());
		}

		/// <summary>
		/// Instantiates new object asscoaited with the specified key.
		/// </summary>
		/// <param name="key">The key of the object to instantiate.</param>
		/// <returns>New instance of the object associated with the specified key.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
		[CanBeNull] 
		public TObject Create([NotNull]TKey key)
		{
			return Resolve(key)();
		}
	}

	/// <summary>
	/// Represents a keyed objects factory.
	/// </summary>
	/// <typeparam name="TKey">The type of key of the object to create.</typeparam>
	/// <typeparam name="TArg">The type of argument that will be passed into object creator.</typeparam>
	/// <typeparam name="TObject">The base type of objects to create.</typeparam>
	public class KeyedFactory<TKey, TArg, TObject> : Container<TKey, Func<TArg, TObject>>
	{
		/// <summary>
		/// Instantiates new object asscoaited with the specified key.
		/// </summary>
		/// <param name="key">The key of the object to instantiate.</param>
		/// <param name="arg">The arg to pass into object creator.</param>
		/// <returns>New instance of the object associated with the specified key.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
		[CanBeNull]
		public TObject Create([NotNull]TKey key, [CanBeNull]TArg arg)
		{
			return Resolve(key)(arg);
		}
	}
}
