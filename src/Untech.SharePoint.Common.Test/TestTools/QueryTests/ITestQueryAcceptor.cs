using System;

namespace Untech.SharePoint.TestTools.QueryTests
{
	public interface ITestQueryAcceptor<T>
	{
		void Accept(ITestQueryExcecutor<T> excecutor);
	}

	public interface ITestQuery<T> : ITestQueryAcceptor<T>
	{
		Type Exception { get; set; }

		string Caml { get; set; }

		string[] ViewFields { get; set; }

		bool EmptyResult { get; set; }
	}
}