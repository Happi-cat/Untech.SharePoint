using System;
using System.Collections.Concurrent;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	internal sealed class MemberAccessorPool
	{
		private readonly ConcurrentDictionary<Type, MemberAccessor> _memberAccessors = new ConcurrentDictionary<Type, MemberAccessor>();

		public static MemberAccessorPool Instance
		{
			get { return Singleton<MemberAccessorPool>.GetInstance(); }
		}

		public MemberAccessor Get<T>()
		{
			return Get(typeof(T));
		}

		public MemberAccessor Get(Type type)
		{
			return _memberAccessors.GetOrAdd(type, CreateMemberAccessor);
		}

		private MemberAccessor CreateMemberAccessor(Type type)
		{
			var accessor = new MemberAccessor();

			accessor.Initialize(type);

			return accessor;
		}
	}
}
