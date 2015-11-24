using System.Diagnostics;
using System.Threading;

namespace Untech.SharePoint.Common.Diagnostics
{
	/// <summary>
	/// Represents class of logging endpoint that writes messages to <see cref="Debugger"/> instance.
	/// </summary>
	public class DebuggerLoggingEndpoint : ILoggingEndpoint
	{
		/// <summary>
		/// Logs message with specified level and category.
		/// </summary>
		/// <param name="level">Logging level of the message.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="message">Message to print.</param>
		public void Log(LogLevel level, string category, string message)
		{
			if (!Debugger.IsAttached)
			{
				return;
			}

			var logMessage = string.Format("[Thread: {0}, Level: {1}, Category: {2}]\n{3}\n\n", 
				Thread.CurrentThread.ManagedThreadId, level, category, message);
			
			Debugger.Log(0, category, logMessage);
		}
	}
}