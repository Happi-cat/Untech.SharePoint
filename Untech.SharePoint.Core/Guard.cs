using System;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core
{
	internal static class Guard
	{
		private const string TypeAssignableFrom = "Type {0} should be assignable from {1}";
		private const string ArrayOrAssignableFromListMessage = "This type should be {0}[] or should be assignable from List<{0}>";
		private const string IsTypeMessage = "This type should be {0}";
		private const string AllowedTypesMessage = "This type should be one from ({0})";

		internal static void ThrowIfArgumentNull(object obj, string paramName)
		{
			if (obj != null) return;

			throw new ArgumentNullException(paramName);
		}

		internal static void ThrowIfArgumentCannotBeAssignedTo<T>(Type type, string paramName)
		{
			if (typeof(T).IsAssignableFrom(type)) return;

			throw new ArgumentException(string.Format(TypeAssignableFrom, typeof(T), type), paramName);
		}

		internal static void ThrowIfArgumentNotArrayOrAssignableFromList<T>(Type type, string paramName)
		{
			if (type.IsArrayOrAssignableFromList<T>()) return;

			throw new ArgumentException(string.Format(ArrayOrAssignableFromListMessage, typeof(T)), paramName);
		}

		internal static void ThrowIfArgumentNotIs<T>(Type type, string paramName)
		{
			if (type.Is<T>()) return;

			throw new ArgumentException(string.Format(IsTypeMessage, typeof(T)), paramName);
		}

		internal static void ThrowIfArgumentNotIs(Type type, Type[] allowedTypes, string paramName)
		{
			if (type.In(allowedTypes)) return;

			throw new ArgumentException(string.Format(AllowedTypesMessage, allowedTypes.JoinToString()), paramName);
		}
	}
}