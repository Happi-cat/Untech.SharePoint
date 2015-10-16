using System;
using System.Collections.Generic;
using System.Linq;
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

		public static bool IsISequence(Type sequenceInterface, Type source, out Type element)
		{
			var type = source.GetInterface(sequenceInterface.Name, false);
			if (type == null && source.IsGenericType && source.GetGenericTypeDefinition() == sequenceInterface)
			{
				type = source;
			}
			if (type == null)
			{
				element = null;
				return false;
			}
			element = type.GetGenericArguments()[0];
			return !element.IsGenericParameter;
		}

		public static bool IsIEnumerable(Type source)
		{
			Type element;
			return IsIEnumerable(source, out element);
		}

		public static bool IsIEnumerable(Type source, out Type element)
		{
			return IsISequence(typeof(IEnumerable<>), source, out element);
		}

		public static bool IsIQueryable(Type source)
		{
			Type element;
			return IsIQueryable(source, out element);
		}

		public static bool IsIQueryable(Type source, out Type element)
		{
			return IsISequence(typeof(IQueryable<>), source, out element);
		}
	}
}
