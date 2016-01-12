using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Tools.Comparers;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public abstract class QueryTest<T>
	{
		#region [Static]

		public static QueryTest<T> Functional<TResult>(Func<IQueryable<T>, TResult> query)
		{
			return new ObjectQueryTest<T,TResult>(query, EqualityComparer<TResult>.Default);
		}

		public static QueryTest<T> Functional<TResult>(Func<IQueryable<T>, IEnumerable<TResult>> query)
		{
			return new SequenceQueryTest<T,TResult>(query, SequenceComparer<TResult>.Default);
		}

		public static QueryTest<T> Functional<TResult>(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			return new ObjectQueryTest<T, TResult>(query, comparer);
		}

		public static QueryTest<T> Functional<TResult>(Func<IQueryable<T>, IEnumerable<TResult>> query,
			IEqualityComparer<IEnumerable<TResult>> comparer)
		{
			return new SequenceQueryTest<T, TResult>(query, comparer);
		}

		public static QueryTest<T> Perfomance<TResult>(Func<IQueryable<T>, TResult> query, string caml)
		{
			return new ObjectQueryPerfTest<T, TResult>(query, caml);
		}

		public static QueryTest<T> Perfomance<TResult>(Func<IQueryable<T>, IEnumerable<TResult>> query, string caml)
		{
			return new SequenceQueryPerfTest<T, TResult>(query, caml);
		}

		#endregion

		public abstract void Accept(QueryTestExecutor<T> executor);

		public QueryTest<T> Throws<TException>()
			where TException : Exception
		{
			return new ExceptionQueryTest<T,TException>(this);
		}
	}

	public class ExceptionQueryTest<T, TException> : QueryTest<T>
		where TException :Exception
	{
		public ExceptionQueryTest(QueryTest<T> inner)
		{
			Inner = inner;
		}

		public QueryTest<T> Inner { get; private set; }

		public Type ExceptionType { get { return typeof (TException); } }

		public override void Accept(QueryTestExecutor<T> executor)
		{
			executor.Execute(this);
		}
	}

	public class ObjectQueryTest<T, TResult> : QueryTest<T>
	{
		public ObjectQueryTest(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			Query = query;
			Comparer = comparer;
		}

		public Func<IQueryable<T>, TResult> Query { get; private set; }

		public IEqualityComparer<TResult> Comparer { get; private set; }

		public override void Accept(QueryTestExecutor<T> executor)
		{
			executor.Execute(this);
		}
	}

	public class SequenceQueryTest<T, TResult> : QueryTest<T>
	{
		public SequenceQueryTest(Func<IQueryable<T>, IEnumerable<TResult>> query, IEqualityComparer<IEnumerable<TResult>> comparer)
		{
			Query = query;
			Comparer = comparer;
		}

		public Func<IQueryable<T>, IEnumerable<TResult>> Query { get; private set; }

		public IEqualityComparer<IEnumerable<TResult>> Comparer { get; private set; }

		public override void Accept(QueryTestExecutor<T> executor)
		{
			executor.Execute(this);
		}
	}

	public class ObjectQueryPerfTest<T, TResult> : QueryTest<T>
	{
		public ObjectQueryPerfTest(Func<IQueryable<T>, TResult> query, string caml)
		{
			Query = query;
			Caml = caml;
		}

		public Func<IQueryable<T>, TResult> Query { get; private set; }

		public string Caml { get; private set; }

		public override void Accept(QueryTestExecutor<T> executor)
		{
			executor.Execute(this);
		}
	}

	public class SequenceQueryPerfTest<T, TResult> : QueryTest<T>
	{
		public SequenceQueryPerfTest(Func<IQueryable<T>, IEnumerable<TResult>> query, string caml)
		{
			Query = query;
			Caml = caml;
		}

		public Func<IQueryable<T>, IEnumerable<TResult>> Query { get; private set; }

		public string Caml { get; private set; }

		public override void Accept(QueryTestExecutor<T> executor)
		{
			executor.Execute(this);
		}
	}
}