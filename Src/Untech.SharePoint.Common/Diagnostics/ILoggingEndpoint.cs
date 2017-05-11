namespace Untech.SharePoint.Diagnostics
{
	/// <summary>
	/// Represents interface of logging endpoint.
	/// </summary>
	public interface ILoggingEndpoint
	{
		/// <summary>
		/// Logs message with specified level and category.
		/// </summary>
		/// <param name="level">Logging level of the message.</param>
		/// <param name="category">Category of the message.</param>
		/// <param name="message">Message to print.</param>
		void Log(LogLevel level, string category, string message);
	}
}