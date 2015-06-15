using System;
using System.Collections.Generic;
using Untech.SharePoint.Core.Utility;

namespace Untech.SharePoint.Core.Reflection
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
			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TObject>(type));
			}
		}

		public TObject Create(Type key)
		{
			return _cachedCreators[key]();
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
			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TArg1, TObject>(type));
			}
		}

		public TObject Create(Type key, TArg1 arg)
		{
			return _cachedCreators[key](arg);
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
			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TArg1, TArg2, TObject>(type));
			}
		}

		public TObject Create(Type key, TArg1 arg1,TArg2 arg2)
		{
			return _cachedCreators[key](arg1, arg2);
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
			if (!_cachedCreators.ContainsKey(type))
			{
				_cachedCreators.Add(type, InstanceCreationUtility.GetCreator<TArg1, TArg2, TArg3, TObject>(type));
			}
		}

		public TObject Create(Type key, TArg1 arg1, TArg2 arg2,TArg3 arg3)
		{
			return _cachedCreators[key](arg1, arg2, arg3);
		}
	}
}