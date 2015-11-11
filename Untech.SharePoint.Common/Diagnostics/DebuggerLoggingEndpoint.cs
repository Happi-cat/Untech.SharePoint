using System.Diagnostics;

namespace Untech.SharePoint.Common.Diagnostics
{
	public class DebuggerLoggingEndpoint : ILoggingEndpoint
	{
		public void Log(LogLevel level, string category, string message)
		{
			Debugger.Log(0, category, message);
		}
	}
}