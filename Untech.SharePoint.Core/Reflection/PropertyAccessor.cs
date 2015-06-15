using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Untech.SharePoint.Core.Reflection
{
	internal class PropertyAccessor
	{
		private readonly Dictionary<string, Action<object, object>> _cachedSetters = new Dictionary<string, Action<object, object>>();

		private readonly Dictionary<string, Func<object, object>> _cachedGetters = new Dictionary<string, Func<object, object>>();

		public void Initialize(Type objectType)
		{
			var properties = objectType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			var fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			foreach (var property in properties)
			{
				RegisterProperty(objectType, property);
			}

			foreach (var field in fields)
			{
				RegisterField(objectType, field);
			}
		}

		public object this[object obj, string propertyName]
		{
			get
			{
				if (_cachedGetters.ContainsKey(propertyName))
				{
					return _cachedGetters[propertyName](obj);
				}
				throw new ArgumentException(string.Format("This property or field '{0}' has no cached getter", propertyName));
			}
			set
			{
				if (_cachedSetters.ContainsKey(propertyName))
				{
					_cachedSetters[propertyName](obj, value);
				}
				throw new ArgumentException(string.Format("This property or field '{0}' has no cached setter", propertyName));
			}
		}

		private void RegisterProperty(Type objectType, PropertyInfo propertyInfo)
		{
			if (propertyInfo.CanRead && !_cachedGetters.ContainsKey(propertyInfo.Name))
			{
				_cachedGetters.Add(propertyInfo.Name, CreateGetter(objectType, propertyInfo.Name));
			}
			if (propertyInfo.CanWrite && !_cachedSetters.ContainsKey(propertyInfo.Name))
			{
				_cachedSetters.Add(propertyInfo.Name, CreateSetter(objectType, propertyInfo.Name, propertyInfo.PropertyType));
			}
		}

		private void RegisterField(Type objectType, FieldInfo fieldInfo)
		{
			if (!_cachedGetters.ContainsKey(fieldInfo.Name))
			{
				_cachedGetters.Add(fieldInfo.Name, CreateGetter(objectType, fieldInfo.Name));
			}
			if (!_cachedSetters.ContainsKey(fieldInfo.Name))
			{
				_cachedSetters.Add(fieldInfo.Name, CreateSetter(objectType, fieldInfo.Name, fieldInfo.FieldType));
			}
		}


		private static Action<object, object> CreateSetter(Type objectType, string propertyName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof(object));
			var valueParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, objectType), propertyName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression.Lambda<Action<object, object>>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private static Func<object, object> CreateGetter(Type objectType, string propertyName)
		{
			var objectParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, objectType), propertyName);

			return Expression.Lambda<Func<object, object>>(propertyExpression, objectParameter)
				.Compile();
		}
	}
}