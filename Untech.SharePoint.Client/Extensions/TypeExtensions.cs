using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Client.Extensions
{
	public static class TypeExtensions
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

	}
}