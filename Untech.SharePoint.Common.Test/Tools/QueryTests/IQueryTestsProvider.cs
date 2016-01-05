using System.Collections.Generic;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public interface IQueryTestsProvider<T>
	{
		IEnumerable<QueryTest<T>> GetQueryTests();
	}
}