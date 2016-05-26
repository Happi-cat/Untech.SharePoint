namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public interface ITestQueryAcceptor<T>
	{
		void Accept(ITestQueryExcecutor<T> excecutor);
	}
}