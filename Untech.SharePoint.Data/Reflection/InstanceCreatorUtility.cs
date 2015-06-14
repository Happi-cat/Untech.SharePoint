using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Untech.SharePoint.Data.Reflection
{
    public static class InstanceCreatorUtility
    {
        public static Func<TResult> GetCreator<TResult>(Type type)
        {
            ShouldImplementOrInherit<TResult>(type);

            return GetCreator<Func<TResult>>(type, new Type[0]);
        }

        public static Func<TArg, TResult> GetCreator<TArg, TResult>(Type type)
        {
            ShouldImplementOrInherit<TResult>(type);

            return GetCreator<Func<TArg, TResult>>(type, new[] {typeof (TArg)});
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

        private static void ShouldImplementOrInherit<TResult>(Type type)
        {
            var resultType = typeof(TResult);

            if (!resultType.IsAssignableFrom(type))
            {
                throw new ArgumentException(string.Format("Type '{0}' should implement or inherit '{1}", type.FullName, resultType.FullName));
            }
        }

        private static TDelegate GetCreator<TDelegate>(Type type, Type[] argumentTypes)
        {
            var defaultConstructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
                CallingConventions.HasThis, argumentTypes, new ParameterModifier[0]);

            var parameterExpressions = argumentTypes.Select(Expression.Parameter).ToList();

            var newExpression = Expression.New(defaultConstructor, parameterExpressions);

            return Expression.Lambda<TDelegate>(newExpression, parameterExpressions).Compile();
        }
    }
}