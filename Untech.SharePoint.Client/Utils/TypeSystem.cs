using System;
using System.Reflection;

namespace Untech.SharePoint.Client.Utils
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
		
		internal static Type GetMemberType(MemberInfo mi)
		{
			var fi = mi as FieldInfo;
			if (fi != null)
			{
				return fi.FieldType;
			}

			var pi = mi as PropertyInfo;
			if (pi != null)
			{
				return pi.PropertyType;
			}

			var ei = mi as EventInfo;
			if (ei != null)
			{
				return ei.EventHandlerType;
			}

			return null;
		}
		

	}
}
