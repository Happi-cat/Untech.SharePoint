using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Getter = System.Func<object, object>;
using Setter = System.Action<object, object>;

namespace Untech.SharePoint.Client.Reflection
{
	internal sealed class PropertyAccessor
	{
		private readonly Dictionary<string, Setter> _cachedSetters = new Dictionary<string, Setter>();

		private readonly Dictionary<string, Getter> _cachedGetters = new Dictionary<string, Getter>();

		public void Initialize(Type objectType)
		{
			Guard.CheckNotNull("objectType", objectType);

			const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

			_cachedGetters.Clear();
			_cachedSetters.Clear();

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
				Guard.CheckNotNull("propertyOrFieldName", propertyOrFieldName);

				if (_cachedGetters.ContainsKey(propertyOrFieldName))
				{
					return _cachedGetters[propertyOrFieldName](obj);
				}
				throw new ArgumentException(string.Format("This property or field '{0}' has no cached getter", propertyOrFieldName));
			}
			set
			{
				Guard.CheckNotNull("propertyOrFieldName", propertyOrFieldName);

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


		private static Setter CreateSetter(Type objectType, string propertyName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof(object));
			var valueParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, objectType), propertyName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression.Lambda<Setter>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private static Getter CreateGetter(Type objectType, string propertyName)
		{
			var objectParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, objectType), propertyName);

			return Expression.Lambda<Getter>(propertyExpression, objectParameter)
				.Compile();
		}
	}
}