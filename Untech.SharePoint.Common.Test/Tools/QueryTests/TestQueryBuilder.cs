using System;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Tools.Comparers;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class TestQueryBuilder<T> : ITestQueryAcceptor<T>
	{
		public TestQueryBuilder(Func<IQueryable<T>, object> query)
		{
			Query = query;
			var comparerAttribute = query.Method.GetCustomAttribute<QueryComparerAttribute>();
			if (comparerAttribute != null)
			{
				Comparer = comparerAttribute.Comparer;
			}
			else
			{
				Type element;
				Comparer = ResultType.IsIEnumerable(out element)
					? typeof(SequenceComparer<>).MakeGenericType(element)
					: null;
			}

			var exceptionAttribute = query.Method.GetCustomAttribute<QueryExceptionAttribute>();
			if (exceptionAttribute != null)
			{
				Exception = exceptionAttribute.Exception;
			}

			var camlAttribute = query.Method.GetCustomAttribute<QueryCamlAttribute>();
			if (camlAttribute != null)
			{
				Caml = camlAttribute.Caml;
			}
		}

		public static implicit operator TestQueryBuilder<T>(Func<IQueryable<T>, object> query)
		{
			return new TestQueryBuilder<T>(query);
		}

		public Func<IQueryable<T>, object> Query { get; private set; }

		public Type ResultType { get { return Query.Method.ReturnType; } }

		public Type Comparer { get; private set; }

		public Type Exception { get; private set; }

		public string Caml { get; private set; }

		public void Accept(ITestQueryExcecutor<T> executor)
		{
			var testQueryType = typeof(TestQuery<,>).MakeGenericType(typeof(T), ResultType);
			var comparer = Comparer != null ? Activator.CreateInstance(Comparer) : null;
			var testQuery = (ITestQueryAcceptor<T>)Activator.CreateInstance(testQueryType, Query, comparer, Exception, Caml);

			testQuery.Accept(executor);
		}
	}
}