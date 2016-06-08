using System;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public interface ITestQueryAcceptor<T>
	{
		Type Exception { get; set; }

		string Caml { get; set; }

		string[] ViewFields { get; set; }

		bool EmptyResult { get; set; }

		void Accept(ITestQueryExcecutor<T> excecutor);
	}
}