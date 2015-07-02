using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Core.Caml;
using Untech.SharePoint.Core.Caml.Modifiers;

namespace Untech.SharePoint.Core.Data.Queryable
{
	internal class SpQueryContext
	{
		// Executes the expression tree that is passed to it. 
		internal static object Execute(Expression expression, bool isEnumerable)
		{
			// The expression must represent a query over the data source. 
			if (!IsQueryOverDataSource(expression))
				throw new InvalidProgramException("No query over the data source was specified.");

			var caml = (new CamlTranslator()).Translate(null, expression);

			throw new NotImplementedException();
		}

		private static bool IsQueryOverDataSource(Expression expression)
		{
			// If expression represents an unqueried IQueryable data source instance, 
			// expression is of type ConstantExpression, not MethodCallExpression. 
			return (expression is MethodCallExpression);
		}
	}


	internal class SpQueryProdiver : IQueryProvider
	{
		public IQueryable CreateQuery(Expression expression)
		{
			Type elementType = TypeSystem.GetElementType(expression.Type);
			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(SpQueryableData<>).MakeGenericType(elementType), this, expression);
			}
			catch (System.Reflection.TargetInvocationException tie)
			{
				throw tie.InnerException;
			}
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new SpQueryableData<TElement>(this, expression);
		}

		public object Execute(Expression expression)
		{
			return SpQueryContext.Execute(expression, false);
		}

		public TElement Execute<TElement>(Expression expression)
		{
			bool isEnumerable = (typeof(TElement).Name == "IEnumerable`1");

			return (TElement)SpQueryContext.Execute(expression, isEnumerable);
		}
	}
}