using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Getter = System.Func<object, object>;
using Setter = System.Action<object, object>;

namespace Untech.SharePoint.Client.Utils.Reflection
{
	internal sealed class MemberAccessor
	{
		private readonly Dictionary<string, Setter> _setters = new Dictionary<string, Setter>();

		private readonly Dictionary<string, Getter> _getters = new Dictionary<string, Getter>();

		public Type Type { get; private set; }

		public void Initialize(Type type)
		{
			Guard.CheckNotNull("type", type);

			Type = type;

			_getters.Clear();
			_setters.Clear();

			type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n=>n.GetIndexParameters().Length == 0)
				.ToList()
				.ForEach(CreateGetterAndSetter);

			type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.ToList()
				.ForEach(CreateGetterAndSetter);
		}

		public object this[object instance, string memberName]
		{
			get
			{
				Guard.CheckNotNull("instance", instance);
				Guard.CheckNotNull("memberName", memberName);

				if (CanRead(memberName))
				{
					return _getters[memberName](instance);
				}
				throw new ArgumentException(string.Format("This member '{0}' has no getter", memberName));
			}
			set
			{
				Guard.CheckNotNull("instance", instance);
				Guard.CheckNotNull("memberName", memberName);

				if (!CanWrite(memberName))
				{
					throw new ArgumentException(string.Format("This member '{0}' has no setter", memberName));
				}
				_setters[memberName](instance, value);
			}
		}

		public bool CanRead(string memberName)
		{
			Guard.CheckNotNull("memberName", memberName);

			return _getters.ContainsKey(memberName);
		}

		public bool CanWrite(string memberName)
		{
			Guard.CheckNotNull("memberName", memberName);

			return _setters.ContainsKey(memberName);
		}

		private void CreateGetterAndSetter(PropertyInfo propertyInfo)
		{
			if (propertyInfo.CanRead && !_getters.ContainsKey(propertyInfo.Name))
			{
				_getters.Add(propertyInfo.Name, CreateGetter(propertyInfo.Name));
			}
			if (propertyInfo.CanWrite && !_setters.ContainsKey(propertyInfo.Name))
			{
				_setters.Add(propertyInfo.Name, CreateSetter(propertyInfo.Name, propertyInfo.PropertyType));
			}
		}

		private void CreateGetterAndSetter(FieldInfo fieldInfo)
		{
			if (!_getters.ContainsKey(fieldInfo.Name))
			{
				_getters.Add(fieldInfo.Name, CreateGetter(fieldInfo.Name));
			}
			if (!fieldInfo.IsInitOnly && !fieldInfo.IsLiteral && !_setters.ContainsKey(fieldInfo.Name))
			{
				_setters.Add(fieldInfo.Name, CreateSetter(fieldInfo.Name, fieldInfo.FieldType));
			}
		}

		private Setter CreateSetter(string memberName, Type propertyType)
		{
			var objectParameter = Expression.Parameter(typeof(object));
			var valueParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, Type), memberName);

			var assignExpression = Expression.Assign(propertyExpression, Expression.Convert(valueParameter, propertyType));

			return Expression.Lambda<Setter>(assignExpression, objectParameter, valueParameter)
				.Compile();
		}

		private Getter CreateGetter(string memberName)
		{
			var objectParameter = Expression.Parameter(typeof(object));

			var propertyExpression = Expression.PropertyOrField(Expression.Convert(objectParameter, Type), memberName);

			return Expression.Lambda<Getter>(propertyExpression, objectParameter)
				.Compile();
		}
	}
}