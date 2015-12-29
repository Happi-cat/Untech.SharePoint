using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Untech.SharePoint.Common.Test.Tools
{
	public class ScenarioRunner
	{
		public void Run(Type testType, string title, IEnumerable<IScenario> scenarios)
		{
			var counter = 0;
			foreach (var scenario in scenarios)
			{
				counter++;
				Run(testType, title + " #" + counter, scenario);
			}
		}

		public void Run(Type testType, string title, IScenario scenario)
		{
			try
			{
				RunCore(scenario);
				LogResult(testType, title, scenario.Stopwatch);
			}
			catch (Exception e)
			{
				LogFailure(testType, title, e);
				throw;
			}
		}

		protected virtual void RunCore(IScenario scenario)
		{
			try
			{
				scenario.BeforeRun();
				scenario.Run();
			}
			finally
			{
				scenario.AfterRun();
			}
		}

		protected virtual void LogResult(Type testType, string title, Stopwatch sw)
		{
			
		}

		protected virtual void LogFailure(Type testType, string title, Exception e)
		{
			
		}
	}
}