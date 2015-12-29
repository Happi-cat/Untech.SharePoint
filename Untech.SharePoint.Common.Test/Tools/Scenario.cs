using System.Diagnostics;

namespace Untech.SharePoint.Common.Test.Tools
{
	public abstract class Scenario : IScenario
	{
		protected Scenario()
		{
			Stopwatch = new Stopwatch();
		}

		public Stopwatch Stopwatch { get; private set; }

		public virtual void BeforeRun()
		{
			
		}

		public abstract void Run();

		public virtual void AfterRun()
		{
			
		}
	}
}