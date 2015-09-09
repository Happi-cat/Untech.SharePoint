using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Getter = System.Func<object, object>;
using Setter = System.Action<object, object>;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	public class MemberAccessUtility
	{
		public static Getter CreateGetter(PropertyInfo propertyInfo)
		{
			if (propertyInfo.CanRead)
			{
				return CreateGetter(propertyInfo.DeclaringType, propertyInfo.Name);
			}

			throw new ArgumentException();
		}

		public static Setter CreateSetter(PropertyInfo propertyInfo)
		{
			if (propertyInfo.CanWrite)
			{
				return CreateSetter(propertyInfo.DeclaringType, propertyInfo.Name, propertyInfo.PropertyType);
			}

			throw new ArgumentException();
		}

		public static Getter CreateGetter(FieldInfo fieldInfo)
		{
			return CreateGetter(fieldInfo.DeclaringType, fieldInfo.Name);
		}

		public static Setter CreateSetter(FieldInfo fieldInfo)
		{
			if (!fieldInfo.IsInitOnly && !fieldInfo.IsLiteral)
			{
				return CreateSetter(fieldInfo.DeclaringType, fieldInfo.Name, fieldInfo.FieldType);
			}

			throw new ArgumentException();
		}

		private static Setter CreateSetter(Type declaringType, string memberName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof(object));
			var valueParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression.Lambda<Setter>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private static Getter CreateGetter(Type declaringType, string memberName)
		{
			var objectParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, declaringType), memberName);

			return Expression.Lambda<Getter>(propertyExpression, objectParameter)
				.Compile();
		}
	}
}
