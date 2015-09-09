using System;
using System.Reflection;

namespace Untech.SharePoint.Common.Utils
{
	internal static class TypeSystem
	{
		internal static bool IsNullableType(Type type)
		{
			return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
		internal static bool IsNullAssignable(Type type)
		{
			return !type.IsValueType || IsNullableType(type);
		}
		
		internal static Type GetMemberType(MemberInfo member)
		{
			var fieldInfo = member as FieldInfo;
			if (fieldInfo != null) return fieldInfo.FieldType;

			var propertyInfo = member as PropertyInfo;
			if (propertyInfo != null) return propertyInfo.PropertyType;

			var eventInfo = member as EventInfo;
			if (eventInfo != null) return eventInfo.EventHandlerType;

			return null;
		}
	}
}
