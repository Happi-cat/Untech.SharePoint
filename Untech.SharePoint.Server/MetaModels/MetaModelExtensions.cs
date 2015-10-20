using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Server.MetaModels
{
	public static class MetaModelExtensions
	{
		private const string MemberGetterProperty = "MemberGetter";
		private const string MemberSetterProperty = "MemberSetter";

		public static Func<object, object> GetMemberGetter(this MetaField field)
		{
			return field.GetAdditionalProperty<Func<object, object>>(MemberGetterProperty);
		}

		public static Action<object, object> GetMemberSetter(this MetaField field)
		{
			return field.GetAdditionalProperty<Action<object, object>>(MemberSetterProperty);
		}


		public static void SetMemberGetter(this MetaField field, Func<object, object> getter)
		{
			field.SetAdditionalProperty(MemberGetterProperty, getter);
		}

		public static void SetMemberSetter(this MetaField field, Action<object, object> setter)
		{
			field.SetAdditionalProperty(MemberSetterProperty, setter);
		}
	}
}