using System;
using System.Reflection;

namespace Untech.SharePoint.Common.Extensions
{
	public static class ReflectionExtensions
	{
		public static bool IsNullableType(this Type type)
		{
			if (!type.IsValueType)
			{
				return true;
			}

			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static bool Is<T>(this Type type)
		{
			return type == typeof (T);
		}

		public static bool IsDefined<T>(this MemberInfo member)
		{
			return member.IsDefined(typeof(T));
		}
	}
}