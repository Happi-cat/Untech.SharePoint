using System;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class NotSupportedQueryAttribute : QueryExceptionAttribute
	{
		public NotSupportedQueryAttribute()
			:base(typeof(NotSupportedException))
		{
		}
	}
}