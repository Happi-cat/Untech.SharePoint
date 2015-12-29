using System;
using System.Diagnostics;
using System.IO;
using Untech.SharePoint.Common.Test.Tools;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class PerfomanceScenarioRunner :ScenarioRunner
	{
		public static readonly int Attempts = 10 * 1000;
		private readonly string _logFilePath;

		public PerfomanceScenarioRunner(string folder)
		{
			if (Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			var fileName = string.Format("PerfLog-{0}.csv", DateTime.Now);
			_logFilePath = Path.Combine(folder, fileName);

			CreateFile();
		}

		protected override void RunCore(IScenario scenario)
		{
			var attempt = 0;
			try
			{
				scenario.BeforeRun();
				for (; attempt < Attempts; attempt++)
				{
					scenario.Run();
				}
				scenario.AfterRun();
			}
			catch
			{
				scenario.AfterRun();
			}
		}

		protected override void LogResult(Type testType, string title, Stopwatch sw)
		{
			using (var file = File.AppendText(_logFilePath))
			{
				file.WriteLine("{0};{1};{2};{3};{4};{5}", testType, title, "OK", Attempts, sw.ElapsedTicks, sw.Elapsed);
			}
		}

		protected override void LogFailure(Type testType, string title, Exception e)
		{
			using (var file = File.AppendText(_logFilePath))
			{
				file.WriteLine("{0};{1};{2};{3};{4};{5}", testType, title, "FAIL", 0, null, null);
			}
		}

		private void CreateFile()
		{
			using (var file = File.CreateText(_logFilePath))
			{
				file.WriteLine("Type;Title;Status;Attempts;Ticks;Timespan");
			}
		}
	}
}