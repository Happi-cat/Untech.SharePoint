using System.Diagnostics;

namespace Untech.SharePoint.Common.Test.Tools
{
	public interface IScenario
	{
		Stopwatch Stopwatch { get; }

		void BeforeRun();

		void Run();

		void AfterRun();
	}
}