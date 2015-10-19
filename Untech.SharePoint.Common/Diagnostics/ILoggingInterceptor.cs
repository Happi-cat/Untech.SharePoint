using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Diagnostics
{
	public interface ILoggingInterceptor
	{
		void Log(Expression nodeBefore, Expression nodeAfter);

		void Log(QueryModel camlBefore, string camlAfter);
	}
}