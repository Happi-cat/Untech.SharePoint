﻿using System;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	[AttributeUsage(AttributeTargets.Method)]
	public class QueryComparerAttribute : Attribute
	{
		public QueryComparerAttribute(Type comparer)
		{
			Comparer = comparer;
		}

		public Type Comparer { get; private set; }
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class QueryCamlAttribute : Attribute
	{
		public QueryCamlAttribute(string caml, string viewFields)
		{
			Caml = caml;
			ViewFields = viewFields.Split(',');
		}

		public string Caml { get; private set; }

		public string[] ViewFields { get; private set; }
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class EmptyResultQueryAttribute : Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Method)]
	public class QueryExceptionAttribute : Attribute
	{
		public QueryExceptionAttribute(Type exception)
		{
			Exception = exception;
		}

		public Type Exception { get; private set; }
	}

	public class NotSupportedQueryAttribute : QueryExceptionAttribute
	{
		public NotSupportedQueryAttribute()
			: base(typeof(NotSupportedException))
		{
		}
	}
}
