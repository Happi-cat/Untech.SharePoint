using System;

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
}