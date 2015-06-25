using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Core.Extensions
{
	public static class TypeExtensions
	{
		public static bool IsNullableType(this Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
		}

		public static bool IsArrayOrAssignableFromList<T>(this Type type)
		{
			return type == typeof(T[]) || type.IsAssignableFrom(typeof(List<T>));
		}

		public static bool Is<T>(this Type type)
		{
			return type == typeof (T);
		}

	}
}