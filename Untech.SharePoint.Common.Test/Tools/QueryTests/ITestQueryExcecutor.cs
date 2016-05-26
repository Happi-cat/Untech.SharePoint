namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public interface ITestQueryExcecutor<T>
	{
		void Visit<TResult>(TestQuery<T, TResult> query);
	}
}