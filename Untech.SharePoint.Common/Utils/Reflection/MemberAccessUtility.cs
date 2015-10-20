using System;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;
using Getter = System.Func<object, object>;
using Setter = System.Action<object, object>;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	public class MemberAccessUtility
	{
		public static Getter CreateGetter(MemberInfo memberInfo)
		{
			var property = memberInfo as PropertyInfo;
			if (property != null) 
			{
				return CreateGetter(property);
			}

			var field = memberInfo as FieldInfo;
			if (field != null)
			{
				return CreateGetter(field);
			}

			throw new ArgumentException();
		}


		public static Setter CreateSetter(MemberInfo memberInfo)
		{
			var property = memberInfo as PropertyInfo;
			if (property != null)
			{
				return CreateSetter(property);
			}

			var field = memberInfo as FieldInfo;
			if (field != null)
			{
				return CreateSetter(field);
			}

			throw new ArgumentException();
		}


		public static Getter CreateGetter(PropertyInfo propertyInfo)
		{
			if (CanCreateGetter(propertyInfo))
			{
				return CreateGetter(propertyInfo.DeclaringType, propertyInfo.Name);
			}

			throw new ArgumentException();
		}

		private static bool CanCreateGetter(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetIndexParameters().IsNullOrEmpty() && propertyInfo.CanRead;
		}

		public static Setter CreateSetter(PropertyInfo propertyInfo)
		{
			if (CanCreateSetter(propertyInfo))
			{
				return CreateSetter(propertyInfo.DeclaringType, propertyInfo.Name, propertyInfo.PropertyType);
			}

			throw new ArgumentException();
		}

		private static bool CanCreateSetter(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetIndexParameters().IsNullOrEmpty() && propertyInfo.CanWrite;
		}

		public static Getter CreateGetter(FieldInfo fieldInfo)
		{
			return CreateGetter(fieldInfo.DeclaringType, fieldInfo.Name);
		}

		public static Setter CreateSetter(FieldInfo fieldInfo)
		{
			if (CanCreateSetter(fieldInfo))
			{
				return CreateSetter(fieldInfo.DeclaringType, fieldInfo.Name, fieldInfo.FieldType);
			}

			throw new ArgumentException();
		}

		private static bool CanCreateSetter(FieldInfo fieldInfo)
		{
			return !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;
		}

		private static Setter CreateSetter(Type declaringType, string memberName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof(object), "object");
			var valueParameter = Expression.Parameter(typeof(object), "value");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression.Lambda<Setter>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private static Getter CreateGetter(Type declaringType, string memberName)
		{
			var objectParameter = Expression.Parameter(typeof(object), "object");

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			return Expression.Lambda<Getter>(Expression.Convert(propertyExpression, typeof(object)), objectParameter)
				.Compile();
		}
	}
}
