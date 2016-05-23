namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public interface ITestQueryAcceptor<in T>
	{
		void Accept(ITestQueryExcecutor<T> excecutor);
	}
}