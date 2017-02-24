using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	internal static class InstanceCreationUtility
	{
		public static Func<TResult> GetCreator<TResult>(Type type)
		{
			Guard.CheckNotNull(nameof(type), type);
			Guard.CheckIsTypeAssignableTo<TResult>(nameof(type), type);

			return GetCreator<Func<TResult>>(type, new Type[0]);
		}

		public static Func<TArg, TResult> GetCreator<TArg, TResult>(Type type)
		{
			Guard.CheckNotNull(nameof(type), type);
			Guard.CheckIsTypeAssignableTo<TResult>(nameof(type), type);

			return GetCreator<Func<TArg, TResult>>(type, new[] { typeof(TArg) });
		}

		public static Func<TArg1, TArg2, TResult> GetCreator<TArg1, TArg2, TResult>(Type type)
		{
			Guard.CheckNotNull(nameof(type), type);
			Guard.CheckIsTypeAssignableTo<TResult>(nameof(type), type);

			return GetCreator<Func<TArg1, TArg2, TResult>>(type, new[] { typeof(TArg1), typeof(TArg2) });
		}

		public static Func<TArg1, TArg2, TArg3, TResult> GetCreator<TArg1, TArg2, TArg3, TResult>(Type type)
		{
			Guard.CheckNotNull(nameof(type), type);
			Guard.CheckIsTypeAssignableTo<TResult>(nameof(type), type);

			return GetCreator<Func<TArg1, TArg2, TArg3, TResult>>(type, new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3) });
		}

		private static TDelegate GetCreator<TDelegate>(Type type, Type[] argumentTypes)
		{
			var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
				CallingConventions.HasThis, argumentTypes, new ParameterModifier[0]);

			if (constructor == null)
			{
				throw ReflectionError.CtorNotFound(type, argumentTypes);
			}

			var parameterExpressions = argumentTypes.Select(Expression.Parameter).ToList();

			var newExpression = Expression.New(constructor, parameterExpressions);

			return Expression.Lambda<TDelegate>(newExpression, parameterExpressions).Compile();
		}
	}
}