using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Diagnostics
{
	/// <summary>
	/// Represents class that logs any message to specified endpoints.
	/// </summary>
	public sealed class Logger
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Logger"/>.
		/// </summary>
		public Logger()
		{
			Endpoints = new Dictionary<Type, ILoggingEndpoint>();
#if DEBUG
			Endpoints[typeof(DebuggerLoggingEndpoint)] = new DebuggerLoggingEndpoint();
#endif
		}

		/// <summary>
		/// Gets singleton instance of the <see cref="Logger"/>
		/// </summary>
		public static Logger Instance
		{
			get { return Singleton<Logger>.GetInstance(); }
		}

		/// <summary>
		/// Gets dicitonary with currently registered logging enpoints.
		/// </summary>
		[NotNull]
		public Dictionary<Type, ILoggingEndpoint> Endpoints { get; private set; }


		/// <summary>
		/// Logs message with specified level and category.
		/// </summary>
		/// <param name="level">Logging level of the message.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="message">Message to print.</param>
		public static void Log(LogLevel level, string category, string message)
		{
			foreach (var endpoint in Instance.Endpoints.Values.Where(n => n!= null))
			{
				 endpoint.Log(level, category, message);
			}
		}

		/// <summary>
		/// Logs message with specified level and category.
		/// </summary>
		/// <param name="level">Logging level of the message.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="exception">The exception to format.</param>
		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, Exception exception)
		{
			Log(level, category, string.Format(format, exception));
		}

		/// <summary>
		/// Logs message with specified level and category.
		/// </summary>
		/// <param name="level">Logging level of the message.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">The exception to format.</param>
		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, object arg)
		{
			Log(level, category, string.Format(format, arg));
		}

		/// <summary>
		/// Logs message with specified level and category.
		/// </summary>
		/// <param name="level">Logging level of the message.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg0">The exception to format.</param>
		/// <param name="arg1">The exception to format.</param>
		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, object arg0, object arg1)
		{
			Log(level, category, string.Format(format, arg0, arg1));
		}

		/// <summary>
		/// Logs message with specified level and category.
		/// </summary>
		/// <param name="level">Logging level of the message.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more object to foramt.</param>
		[StringFormatMethod("format")]
		public static void Log(LogLevel level, string category, string format, params object[] args)
		{
			Log(level, category, string.Format(format, args));
		}

		[StringFormatMethod("format")]
		internal static void Trace(string category, string format, params object[] args)
		{
			Log(LogLevel.Trace, category, string.Format(format, args));
		}

		[StringFormatMethod("format")]
		internal static void Debug(string category, string format, params object[] args)
		{
			Log(LogLevel.Trace, category, string.Format(format, args));
		}
	}
}