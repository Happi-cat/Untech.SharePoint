using System;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.Translators;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	internal class SpLinqQueryProvider : IQueryProvider
	{
		public static SpLinqQueryProvider Instance => Singleton<SpLinqQueryProvider>.GetInstance();

		public IQueryable CreateQuery(Expression expression)
		{
			Guard.CheckNotNull(nameof(expression), expression);

			Type element;
			if (!expression.Type.IsIEnumerable(out element))
			{
				throw new ArgumentException("Invalid expression type", nameof(expression));
			}

			// ReSharper disable once PossibleNullReferenceException
			return (IQueryable)typeof(SpLinqQuery<>).MakeGenericType(element)
				.GetConstructor(new[] { typeof(Expression) })
				.Invoke(new object[] { expression });
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			Guard.CheckNotNull(nameof(expression), expression);

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

		private Func<T> RewriteAndCompile<T>(Expression expression)
		{
			Guard.CheckNotNull(nameof(expression), expression);
			Guard.CheckIsTypeAssignableTo<T>(nameof(expression), expression.Type);

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