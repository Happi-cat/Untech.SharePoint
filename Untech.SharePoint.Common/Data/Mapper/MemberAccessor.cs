using System;
using System.Reflection;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Data.Mapper
{
	internal class MemberAccessor : IFieldAccessor<object>
	{
		public MemberAccessor(MemberInfo member)
		{
			MemberGetter = MemberAccessUtility.CreateGetter(member);
			MemberSetter = MemberAccessUtility.CreateSetter(member);
		}

		public Func<object, object> MemberGetter { get; private set; }

		public Action<object, object> MemberSetter { get; private set; }


		public bool CanGetValue { get { return MemberGetter != null; } }
		public bool CanSetValue { get { return MemberSetter != null; } }

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