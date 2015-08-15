using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Untech.SharePoint.Client.Reflection
{
	internal class PropertyAccessor
	{
		private readonly Dictionary<string, Action<object, object>> _cachedSetters = new Dictionary<string, Action<object, object>>();

		private readonly Dictionary<string, Func<object, object>> _cachedGetters = new Dictionary<string, Func<object, object>>();

		public void Initialize(Type objectType)
		{
			const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

			var properties = objectType.GetProperties(bindingFlags);
			var fields = objectType.GetFields(bindingFlags);

			foreach (var property in properties)
			{
				RegisterProperty(objectType, property);
			}

			foreach (var field in fields)
			{
				RegisterField(objectType, field);
			}
		}

		public object this[object obj, string propertyOrFieldName]
		{
			get
			{
				if (_cachedGetters.ContainsKey(propertyOrFieldName))
				{
					return _cachedGetters[propertyOrFieldName](obj);
				}
				throw new ArgumentException(string.Format("This property or field '{0}' has no cached getter", propertyOrFieldName));
			}
			set
			{
				if (_cachedSetters.ContainsKey(propertyOrFieldName))
				{
					_cachedSetters[propertyOrFieldName](obj, value);
					return;
				}
				throw new ArgumentException(string.Format("This property or field '{0}' has no cached setter", propertyOrFieldName));
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