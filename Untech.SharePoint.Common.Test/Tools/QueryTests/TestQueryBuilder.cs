using System;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Tools.Comparers;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class TestQueryBuilder<T> : ITestQueryAcceptor<T>
	{
		private readonly QueryComparerAttribute _comparerAttribute;
		private readonly QueryExceptionAttribute _exceptionAttribute;
		private readonly QueryCamlAttribute _camlAttribute;

		public TestQueryBuilder(Func<IQueryable<T>, object> query)
		{
			Query = query;
			_comparerAttribute = query.Method.GetCustomAttribute<QueryComparerAttribute>();
			_exceptionAttribute = query.Method.GetCustomAttribute<QueryExceptionAttribute>();
			_camlAttribute = query.Method.GetCustomAttribute<QueryCamlAttribute>();
			EmptyResult = query.Method.GetCustomAttribute<EmptyResultQueryAttribute>() != null;
		}

		public static implicit operator TestQueryBuilder<T>(Func<IQueryable<T>, object> query)
		{
			return new TestQueryBuilder<T>(query);
		}

		protected Func<IQueryable<T>, object> Query { get; }

		protected Type ResultType => Query.Method.ReturnType;

		protected Type Comparer
		{
			get
			{
				if (_comparerAttribute != null)
				{
					return _comparerAttribute.Comparer;
				}

				Type element;
				return ResultType.IsIEnumerable(out element)
					? typeof(SequenceComparer<>).MakeGenericType(element)
					: null;
			}
		}

		protected Type Exception => _exceptionAttribute?.Exception;

		protected string Caml => _camlAttribute?.Caml;

		protected string[] ViewFields => _camlAttribute?.ViewFields;

		protected bool EmptyResult { get; }

		public void Accept(ITestQueryExcecutor<T> executor)
		{
			var testQueryType = typeof(TestQuery<,>).MakeGenericType(typeof(T), ResultType);
			var comparer = Comparer != null ? Activator.CreateInstance(Comparer) : null;
			var testQuery = (ITestQuery<T>)Activator.CreateInstance(testQueryType, Query, comparer);

			testQuery.Exception = Exception;
			testQuery.Caml = Caml;
			testQuery.ViewFields = ViewFields;
			testQuery.EmptyResult = EmptyResult;

			testQuery.Accept(executor);
		}
	}
}