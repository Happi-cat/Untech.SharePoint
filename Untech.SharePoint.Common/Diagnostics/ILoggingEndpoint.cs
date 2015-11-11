using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Diagnostics
{
	public interface ILoggingEndpoint
	{
		void Log(LogLevel level, string category, string message);
	}
}