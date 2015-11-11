using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Diagnostics
{
	public sealed class Logger
	{
		public Logger()
		{
			Endpoints = new Dictionary<Type, ILoggingEndpoint>();
#if DEBUG
			Endpoints[typeof(DebuggerLoggingEndpoint)] = new DebuggerLoggingEndpoint();
#endif
		}

		public static Logger Instance
		{
			get { return Singleton<Logger>.GetInstance(); }
		}

		public Dictionary<Type, ILoggingEndpoint> Endpoints { get; private set; }

		public static void Log(LogLevel level, string category, string message)
		{
			if (Instance.Endpoints == null) return;

			foreach (var endpoint in Instance.Endpoints.Values.Where(n => n!= null))
			{
				 endpoint.Log(level, category, message);
			}
		}

		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, Exception exception)
		{
			Log(level, category, string.Format(format, exception));
		}

		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, object arg)
		{
			Log(level, category, string.Format(format, arg));
		}

		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, object arg0, object arg1)
		{
			Log(level, category, string.Format(format, arg0, arg1));
		}

		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, params object[] args)
		{
			Log(level, category, string.Format(format, args));
		}
	}
}