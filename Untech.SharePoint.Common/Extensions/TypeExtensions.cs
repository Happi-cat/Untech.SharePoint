using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Extensions
{
	/// <summary>
	/// Provides a set of static method that can be used with <see cref="Type"/> and <see cref="System.Reflection"/>
	/// </summary>
	[PublicAPI]
	public static class TypeExtensions
	{
		/// <summary>
		/// Determines whether this type is <see cref="Nullable{T}"/>.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <returns>trye if <paramref name="type"/> is <see cref="Nullable{T}"/></returns>
		public static bool IsNullable([CanBeNull]this Type type)
		{
			return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		/// <summary>
		/// Determines whether we can assign null to instance of <paramref name="type"/> <see cref="Type"/>(i.e. <paramref name="type"/> is <see cref="Nullable{T}"/> or is a class. 
		/// </summary>
		/// <param name="type">Type to check</param>
		/// <returns>true if <paramref name="type"/> is <see cref="Nullable{T}"/> or is a class.</returns>
		public static bool IsNullAssignable([CanBeNull]this Type type)
		{
			return type != null && type.IsClass || type.IsNullable();
		}

		/// <summary>
		/// Determines whether <see cref="Type"/> is equal to <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">Type that is expected.</typeparam>
		/// <param name="type">Type to check.</param>
		/// <returns>true if <paramref name="type"/> is equal to <typeparamref name="T"/>.</returns>
		public static bool Is<T>([CanBeNull]this Type type)
		{
			return type == typeof (T);
		}

		/// <summary>
		/// Determines whether <paramref name="source"/> <see cref="Type"/> is <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <param name="source">Type to check.</param>
		/// <returns>true if <paramref name="source"/> is <see cref="IEnumerable{T}"/>; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static bool IsIEnumerable([NotNull]this Type source)
		{
			Type element;
			return IsIEnumerable(source, out element);
		}

		/// <summary>
		/// Determines whether <paramref name="source"/> <see cref="Type"/> is <see cref="IEnumerable{T}"/> and returns element type.
		/// </summary>
		/// <param name="source">Type to check.</param>
		/// <param name="element">Type of sequence element.</param>
		/// <returns>true if <paramref name="source"/> is <see cref="IEnumerable{T}"/>; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static bool IsIEnumerable([NotNull]this Type source, [CanBeNull]out Type element)
		{
			Guard.CheckNotNull("source", source);

			return IsISequence(typeof(IEnumerable<>), source, out element);
		}

		/// <summary>
		/// Determines whether <paramref name="source"/> <see cref="Type"/> is <see cref="IQueryable{T}"/>.
		/// </summary>
		/// <param name="source">Type to check.</param>
		/// <returns>true if <paramref name="source"/> is <see cref="IQueryable{T}"/>; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static bool IsIQueryable([NotNull]this Type source)
		{
			Type element;
			return IsIQueryable(source, out element);
		}

		/// <summary>
		/// Determines whether <paramref name="source"/> <see cref="Type"/> is <see cref="IQueryable{T}"/> and returns element type.
		/// </summary>
		/// <param name="source">Type to check.</param>
		/// <param name="element">Type of sequence element.</param>
		/// <returns>true if <paramref name="source"/> is <see cref="IQueryable{T}"/>; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static bool IsIQueryable([NotNull]this Type source, [CanBeNull]out Type element)
		{
			Guard.CheckNotNull("source", source);

			return IsISequence(typeof(IQueryable<>), source, out element);
		}

		/// <summary>
		/// Gets member type of the specified <see cref="MemberInfo"/>.
		/// </summary>
		/// <param name="member">MemberInfo which member type we should get.</param>
		/// <returns>
		/// Returns <see cref="FieldInfo.FieldType"/> for <see cref="FieldInfo"/> member, 
		/// <see cref="PropertyInfo.PropertyType"/> for <see cref="PropertyInfo"/> member,
		/// <see cref="EventInfo.EventHandlerType"/> for <see cref="EventInfo"/> member,
		/// <see cref="MethodInfo.ReturnType"/> for <see cref="MethodInfo"/> member.
		/// </returns>
		[NotNull]
		public static Type GetMemberType([CanBeNull]this MemberInfo member)
		{
			var fieldInfo = member as FieldInfo;
			if (fieldInfo != null) return fieldInfo.FieldType;

			var propertyInfo = member as PropertyInfo;
			if (propertyInfo != null) return propertyInfo.PropertyType;

			var eventInfo = member as EventInfo;
			if (eventInfo != null) return eventInfo.EventHandlerType;

			var methodInfo = member as MethodInfo;
			if (methodInfo != null) return methodInfo.ReturnType;

			throw new ArgumentException("Invalid memberinfo", "member");
		}

		#region [Private Methods]

		private static bool IsISequence([NotNull]Type sequenceInterface, [NotNull]Type source, [CanBeNull]out Type element)
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

		#endregion


	}
}