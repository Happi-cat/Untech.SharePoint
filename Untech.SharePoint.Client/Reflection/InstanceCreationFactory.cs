using System;
using System.Collections.Generic;
using Untech.SharePoint.Client.Utility;

namespace Untech.SharePoint.Client.Reflection
{
	public class InstanceCreationFactory<TObject>
	{
		private readonly Dictionary<Type, Func<TObject>> _cachedCreators = new Dictionary<Type, Func<TObject>>();

		public static InstanceCreationFactory<TObject> Instance
		{
			get { return Singleton<InstanceCreationFactory<TObject>>.GetInstance(); }
		}

		public void Register(Type type)
		{
			Guard.CheckNotNull("type", type);

			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TObject>(type));
			}
		}

		public TObject Create(Type type)
		{
			Guard.CheckNotNull("type", type);

			return _cachedCreators[type]();
		}
	}

	public class InstanceCreationFactory<TArg1, TObject>
	{
		private readonly Dictionary<Type, Func<TArg1, TObject>> _cachedCreators = new Dictionary<Type, Func<TArg1, TObject>>();

		public static InstanceCreationFactory<TArg1, TObject> Instance
		{
			get { return Singleton<InstanceCreationFactory<TArg1, TObject>>.GetInstance(); }
		}

		public void Register(Type type)
		{
			Guard.CheckNotNull("type", type);

			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TArg1, TObject>(type));
			}
		}

		public TObject Create(Type type, TArg1 arg)
		{
			Guard.CheckNotNull("type", type);

			return _cachedCreators[type](arg);
		}
	}

	public class InstanceCreationFactory<TArg1, TArg2, TObject>
	{
		private readonly Dictionary<Type, Func<TArg1, TArg2, TObject>> _cachedCreators = new Dictionary<Type, Func<TArg1, TArg2, TObject>>();

		public static InstanceCreationFactory<TArg1, TArg2, TObject> Instance
		{
			get { return Singleton<InstanceCreationFactory<TArg1, TArg2, TObject>>.GetInstance(); }
		}

		public void Register(Type type)
		{
			Guard.CheckNotNull("type", type);

			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TArg1, TArg2, TObject>(type));
			}
		}

		public TObject Create(Type type, TArg1 arg1,TArg2 arg2)
		{
			Guard.CheckNotNull("type", type);

			return _cachedCreators[type](arg1, arg2);
		}
	}

	public class InstanceCreationFactory<TArg1, TArg2, TArg3, TObject>
	{
		private readonly Dictionary<Type, Func<TArg1, TArg2, TArg3, TObject>> _cachedCreators = new Dictionary<Type, Func<TArg1, TArg2, TArg3, TObject>>();

		public static InstanceCreationFactory<TArg1, TArg2, TArg3, TObject> Instance
		{
			get { return Singleton<InstanceCreationFactory<TArg1, TArg2, TArg3, TObject>>.GetInstance(); }
		}

		public void Register(Type type)
		{
			Guard.CheckNotNull("type", type);

			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TArg1, TArg2, TArg3, TObject>(type));
			}
		}

		public TObject Create(Type type, TArg1 arg1, TArg2 arg2,TArg3 arg3)
		{
			Guard.CheckNotNull("type", type);

			return _cachedCreators[type](arg1, arg2, arg3);
		}
	}
}