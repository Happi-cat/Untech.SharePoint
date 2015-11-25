using System;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;
using Getter = System.Func<object, object>;
using Setter = System.Action<object, object>;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	internal static class MemberAccessUtility
	{
		#region [Create Getter]

		public static Getter CreateGetter(MemberInfo memberInfo)
		{
			var property = memberInfo as PropertyInfo;
			if (property != null) return CreateGetter(property);

			var field = memberInfo as FieldInfo;
			if (field != null) return CreateGetter(field);

			throw ReflectionError.CannotCreateGetter(memberInfo);
		}

		public static Getter CreateGetter(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.GetIndexParameters().IsNullOrEmpty())
			{
				throw ReflectionError.CannotCreateGetterForIndexer(propertyInfo);
			}
			if (!propertyInfo.CanRead)
			{
				throw ReflectionError.CannotCreateGetterForWriteOnly(propertyInfo);
			}
			return CreateGetter(propertyInfo.DeclaringType, propertyInfo.Name);
		}

		public static Getter CreateGetter(FieldInfo fieldInfo)
		{
			return CreateGetter(fieldInfo.DeclaringType, fieldInfo.Name);
		}

		#endregion

		#region [Create Setter]

		public static Setter CreateSetter(MemberInfo memberInfo)
		{
			var property = memberInfo as PropertyInfo;
			if (property != null) return CreateSetter(property);

			var field = memberInfo as FieldInfo;
			if (field != null) return CreateSetter(field);

			throw ReflectionError.CannotCreateSetter(memberInfo);
		}

		public static Setter CreateSetter(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.GetIndexParameters().IsNullOrEmpty())
			{
				throw ReflectionError.CannotCreateGetterForIndexer(propertyInfo);
			}
			if (!propertyInfo.CanWrite)
			{
				throw ReflectionError.CannotCreateSetterForReadOnly(propertyInfo);
			}
			return CreateSetter(propertyInfo.DeclaringType, propertyInfo.Name, propertyInfo.PropertyType);
		}

		public static Setter CreateSetter(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
			{
				throw ReflectionError.CannotCreateSetterForReadOnly(fieldInfo);
			}
			return CreateSetter(fieldInfo.DeclaringType, fieldInfo.Name, fieldInfo.FieldType);
		}

		#endregion

		#region [Private Methods]

		private static Setter CreateSetter(Type declaringType, string memberName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof (object), "object");
			var valueParameter = Expression.Parameter(typeof (object), "value");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression.Lambda<Setter>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private static Getter CreateGetter(Type declaringType, string memberName)
		{
			var objectParameter = Expression.Parameter(typeof (object), "object");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			return Expression.Lambda<Getter>(Expression.Convert(propertyExpression, typeof (object)), objectParameter)
				.Compile();
		}

		#endregion

	}
}
