using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	public sealed class InstanceCreationFactory<TObject>
	{
		private readonly Dictionary<Type, Func<TObject>> _cachedCreators = new Dictionary<Type, Func<TObject>>();

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

	public sealed class InstanceCreationFactory<TArg1, TObject>
	{
		private readonly Dictionary<Type, Func<TArg1, TObject>> _cachedCreators = new Dictionary<Type, Func<TArg1, TObject>>();

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

	public sealed class InstanceCreationFactory<TArg1, TArg2, TObject>
	{
		private readonly Dictionary<Type, Func<TArg1, TArg2, TObject>> _cachedCreators = new Dictionary<Type, Func<TArg1, TArg2, TObject>>();

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

	public sealed class InstanceCreationFactory<TArg1, TArg2, TArg3, TObject>
	{
		private readonly Dictionary<Type, Func<TArg1, TArg2, TArg3, TObject>> _cachedCreators = new Dictionary<Type, Func<TArg1, TArg2, TArg3, TObject>>();

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