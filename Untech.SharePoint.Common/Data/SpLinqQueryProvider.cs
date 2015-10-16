using System;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.Translators;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	internal class SpLinqQueryProvider : IQueryProvider
	{
		public static SpLinqQueryProvider Instance
		{
			get { return Singleton<SpLinqQueryProvider>.GetInstance(); }
		}

		public IQueryable CreateQuery(Expression expression)
		{
			Guard.CheckNotNull("expression", expression);

			Type element;
			if (!TypeSystem.IsIEnumerable(expression.Type, out element))
			{
				throw new ArgumentException("Invalid expression type", "expression");
			}

			// ReSharper disable once PossibleNullReferenceException
			return (IQueryable)typeof(SpLinqQuery<>).MakeGenericType(element)
				.GetConstructor(new[] { typeof(Expression) })
				.Invoke(new object[] { expression });
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			Guard.CheckNotNull("expression", expression);

			return new SpLinqQuery<TElement>(expression);
		}

		public object Execute(Expression expression)
		{
			return RewriteAndCompile<object>(expression)();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return RewriteAndCompile<TResult>(expression)();
		}

		internal static Func<T> RewriteAndCompile<T>(Expression expression)
		{
			Guard.CheckNotNull("expression", expression);
			Guard.CheckTypeIsAssignableTo<T>("expression", expression.Type);

			return Compile<T>(Rewrite(expression));
		}

		private static Expression Rewrite(Expression expression)
		{
			return new CamlQueryTreeProcessor().Process(expression);
		}

		private static Func<T> Compile<T>(Expression expression)
		{
			return Expression.Lambda<Func<T>>(expression).Compile();
		}
	}
}