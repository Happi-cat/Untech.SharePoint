using System;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Extensions;
using Getter = System.Func<object, object>;
using Setter = System.Action<object, object>;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	internal static class MemberAccessUtility
	{
		#region [Create Getter]

		public static Getter CreateGetter(MemberInfo memberInfo)
		{
			return CreateGetter<object, object>(memberInfo);
		}

		public static Func<TObj, TProp> CreateGetter<TObj, TProp>(MemberInfo memberInfo)
		{
			var property = memberInfo as PropertyInfo;
			if (property != null) return CreateGetter<TObj, TProp>(property);

			var field = memberInfo as FieldInfo;
			if (field != null) return CreateGetter<TObj, TProp>(field);

			throw ReflectionError.CannotCreateGetter(memberInfo);
		}

		private static Func<TObj, TProp> CreateGetter<TObj, TProp>(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.GetIndexParameters().IsNullOrEmpty())
			{
				throw ReflectionError.CannotCreateGetterForIndexer(propertyInfo);
			}
			if (!propertyInfo.CanRead)
			{
				throw ReflectionError.CannotCreateGetterForWriteOnly(propertyInfo);
			}
			return CreateGetter<TObj, TProp>(propertyInfo.DeclaringType, propertyInfo.Name);
		}

		private static Func<TObj, TProp> CreateGetter<TObj, TProp>(FieldInfo fieldInfo)
		{
			return CreateGetter<TObj, TProp>(fieldInfo.DeclaringType, fieldInfo.Name);
		}

		#endregion

		#region [Create Setter]

		public static Action<object, object> CreateSetter(MemberInfo memberInfo)
		{
			return CreateSetter<object, object>(memberInfo);
		}

		public static Action<TObj, TProp> CreateSetter<TObj, TProp>(MemberInfo memberInfo)
		{
			var property = memberInfo as PropertyInfo;
			if (property != null) return CreateSetter<TObj, TProp>(property);

			var field = memberInfo as FieldInfo;
			if (field != null) return CreateSetter<TObj, TProp>(field);

			throw ReflectionError.CannotCreateSetter(memberInfo);
		}

		private static Action<TObj, TProp> CreateSetter<TObj, TProp>(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.GetIndexParameters().IsNullOrEmpty())
			{
				throw ReflectionError.CannotCreateGetterForIndexer(propertyInfo);
			}
			if (!propertyInfo.CanWrite)
			{
				throw ReflectionError.CannotCreateSetterForReadOnly(propertyInfo);
			}
			return CreateSetter<TObj, TProp>(propertyInfo.DeclaringType, propertyInfo.Name, propertyInfo.PropertyType);
		}

		private static Action<TObj, TProp> CreateSetter<TObj, TProp>(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
			{
				throw ReflectionError.CannotCreateSetterForReadOnly(fieldInfo);
			}
			return CreateSetter<TObj, TProp>(fieldInfo.DeclaringType, fieldInfo.Name, fieldInfo.FieldType);
		}

		#endregion

		#region [Private Methods]

		private static Action<TObj, TProp> CreateSetter<TObj, TProp>(Type declaringType, string memberName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof (TObj), "object");
			var valueParameter = Expression.Parameter(typeof (TProp), "value");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression
				.Lambda<Action<TObj, TProp>>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private static Func<TObj, TProp> CreateGetter<TObj, TProp>(Type declaringType, string memberName)
		{
			var objectParameter = Expression.Parameter(typeof (TObj), "object");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			return Expression.Lambda<Func<TObj, TProp>>(Expression.Convert(propertyExpression, typeof(TProp)), objectParameter)
				.Compile();
		}

		#endregion

	}
}
