using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Diagnostics
{
	/// <summary>
	/// Describes different levels of logging message.
	/// </summary>
	[PublicAPI]
	public enum LogLevel
	{
		/// <summary>
		/// Not specified.
		/// </summary>
		None = 0,
		/// <summary>
		/// More detailed information than <see cref="Debug"/> for logs only.
		/// </summary>
		Trace,
		/// <summary>
		/// Detailed information on the flow through the system for logs only.
		/// </summary>
		Debug,
		/// <summary>
		/// Interesting runtime events (startup/shutdown).
		/// </summary>
		Info,
		/// <summary>
		/// Runtime situations that are undesirable or unexpected, but not necessarily "wrong".
		/// </summary>
		Warning,
		/// <summary>
		/// Other runtime errors or unexpected conditions.
		/// </summary>
		Error
	}
}