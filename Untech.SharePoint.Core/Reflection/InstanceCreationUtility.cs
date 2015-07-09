using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Reflection
{
	public static class InstanceCreationUtility
	{
		public static Func<TResult> GetCreator<TResult>(Type type)
		{
			ShouldImplementOrInherit<TResult>(type);

			return GetCreator<Func<TResult>>(type, new Type[0]);
		}

		public static Func<TArg, TResult> GetCreator<TArg, TResult>(Type type)
		{
			ShouldImplementOrInherit<TResult>(type);

			return GetCreator<Func<TArg, TResult>>(type, new[] { typeof(TArg) });
		}

		public static Func<TArg1, TArg2, TResult> GetCreator<TArg1, TArg2, TResult>(Type type)
		{
			ShouldImplementOrInherit<TResult>(type);

			return GetCreator<Func<TArg1, TArg2, TResult>>(type, new[] { typeof(TArg1), typeof(TArg2) });
		}

		public static Func<TArg1, TArg2, TArg3, TResult> GetCreator<TArg1, TArg2, TArg3, TResult>(Type type)
		{
			ShouldImplementOrInherit<TResult>(type);

			return GetCreator<Func<TArg1, TArg2, TArg3, TResult>>(type, new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) });
		}

		private static void ShouldImplementOrInherit<TBase>(Type type)
		{
			var baseType = typeof(TBase);

			if (!baseType.IsAssignableFrom(type))
			{
				throw new ArgumentException(string.Format("Type '{0}' should implement or inherit '{1}", type.FullName, baseType.FullName));
			}
		}

		private static TDelegate GetCreator<TDelegate>(Type type, Type[] argumentTypes)
		{
			var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
				CallingConventions.HasThis, argumentTypes, new ParameterModifier[0]);

			ShouldHaveConstructor(type, argumentTypes, constructor);

			var parameterExpressions = argumentTypes.Select(Expression.Parameter).ToList();

			var newExpression = Expression.New(constructor, parameterExpressions);

			return Expression.Lambda<TDelegate>(newExpression, parameterExpressions).Compile();
		}

		private static void ShouldHaveConstructor(Type type, Type[] argumentTypes, ConstructorInfo ctor)
		{
			if (ctor != null) return;

			throw new ArgumentException(string.Format("Type {0} has no constructor that matches parameters list ({1})", type, argumentTypes.JoinToString()));
		}
	}
}