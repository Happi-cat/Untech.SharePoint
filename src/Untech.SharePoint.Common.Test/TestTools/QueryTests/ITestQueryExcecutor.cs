namespace Untech.SharePoint.TestTools.QueryTests
{
	public interface ITestQueryExcecutor<T>
	{
		void Visit<TResult>(TestQuery<T, TResult> query);
	}
}