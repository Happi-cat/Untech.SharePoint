using System;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	[AttributeUsage(AttributeTargets.Method)]
	public class QueryExceptionAttribute : Attribute
	{
		public QueryExceptionAttribute(Type exception)
		{
			Exception = exception;
		}

		public Type Exception { get; private set; }
	}
}