using System;
using System.Reflection;
using Untech.SharePoint.Utils.Reflection;

namespace Untech.SharePoint.Data.Mapper
{
	internal class MemberAccessor : IFieldAccessor<object>
	{
		public MemberAccessor(MemberInfo member)
		{
			MemberGetter = MemberAccessUtility.CreateGetter(member);
			MemberSetter = MemberAccessUtility.CreateSetter(member);
		}

		private Func<object, object> MemberGetter { get; }

		private Action<object, object> MemberSetter { get; }

		public bool CanGetValue => MemberGetter != null;

		public bool CanSetValue => MemberSetter != null;

		public object GetValue(object instance)
		{
			return MemberGetter(instance);
		}

		public void SetValue(object instance, object value)
		{
			MemberSetter(instance, value);
		}
	}
}