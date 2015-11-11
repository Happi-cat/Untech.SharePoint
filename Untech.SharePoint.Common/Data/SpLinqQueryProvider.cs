using System;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.Translators;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.Extensions;
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
			if (!expression.Type.IsIEnumerable(out element))
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

		private Func<T> RewriteAndCompile<T>(Expression expression)
		{
			Guard.CheckNotNull("expression", expression);
			Guard.CheckIsTypeAssignableTo<T>("expression.Type", expression.Type);

			return Compile<T>(Rewrite(expression));
		}

		private static Expression Rewrite(Expression expression)
		{
			Logger.Log(LogLevel.Info, LogCategories.Expression, "Expression before rewrite: {0}", expression);

			return new CamlQueryTreeProcessor().Process(expression);
		}

		private static Func<T> Compile<T>(Expression expression)
		{
			Logger.Log(LogLevel.Info, LogCategories.Expression, "Expression before compilation: {0}", expression);

			return Expression.Lambda<Func<T>>(expression).Compile();
		}
	}
}