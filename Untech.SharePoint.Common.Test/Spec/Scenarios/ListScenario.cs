using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Test.Tools;

namespace Untech.SharePoint.Common.Test.Spec.Scenarios
{
	public abstract class ListScenario<T> : Scenario
	{
		protected ListScenario(ISpList<T> list)
		{
			List = list;
		}

		public ISpList<T> List { get; private set; }
	}
}