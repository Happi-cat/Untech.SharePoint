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
				ViewFields = camlAttribute.ViewFields;
			}

			EmptyResult = query.Method.GetCustomAttribute<EmptyResultQueryAttribute>() != null;
		}

		public static implicit operator TestQueryBuilder<T>(Func<IQueryable<T>, object> query)
		{
			return new TestQueryBuilder<T>(query);
		}

		public Func<IQueryable<T>, object> Query { get; private set; }

		public Type ResultType { get { return Query.Method.ReturnType; } }

		public Type Comparer { get; set; }

		public Type Exception { get; set; }

		public string Caml { get; set; }

		public string[] ViewFields { get; set; }
		public bool EmptyResult { get; set; }

		public void Accept(ITestQueryExcecutor<T> executor)
		{
			var testQueryType = typeof(TestQuery<,>).MakeGenericType(typeof(T), ResultType);
			var comparer = Comparer != null ? Activator.CreateInstance(Comparer) : null;
			var testQuery = (ITestQueryAcceptor<T>)Activator.CreateInstance(testQueryType, Query, comparer);

			testQuery.Exception = Exception;
			testQuery.Caml = Caml;
			testQuery.ViewFields = ViewFields;
			testQuery.EmptyResult = EmptyResult;

			testQuery.Accept(executor);
		}
	}
}