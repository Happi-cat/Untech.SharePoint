using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools;

namespace Untech.SharePoint.Client.Test.Data
{
	[TestClass]
	public class ServerBasicListOperationsTest : BasicListOperationsTest
	{
		public ServerBasicListOperationsTest() : base(GetContext())
		{
		}

		public ServerBasicListOperationsTest(ScenarioRunner runner) : base(GetContext(), runner)
		{
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}

	[TestClass]
	public class ServerAggregateListOperationsTest : AggregateListOperationsTest
	{
		public ServerAggregateListOperationsTest()
			: base(GetContext())
		{
		}

		public ServerAggregateListOperationsTest(ScenarioRunner runner)
			: base(GetContext(), runner)
		{
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}

	[TestClass]
	public class ServerFilteringListOperationsTest : FilteringListOperationsTest
	{
		public ServerFilteringListOperationsTest()
			: base(GetContext())
		{
		}

		public ServerFilteringListOperationsTest(ScenarioRunner runner)
			: base(GetContext(), runner)
		{
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}

	[TestClass]
	public class ServerOrderingListOperationsTest : OrderingListOperationsTest
	{
		public ServerOrderingListOperationsTest()
			: base(GetContext())
		{
		}

		public ServerOrderingListOperationsTest(ScenarioRunner runner)
			: base(GetContext(), runner)
		{
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}

	[TestClass]
	public class ServerPagingListOperationsTest : PagingListOperationsTest
	{
		public ServerPagingListOperationsTest()
			: base(GetContext())
		{
		}

		public ServerPagingListOperationsTest(ScenarioRunner runner)
			: base(GetContext(), runner)
		{
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}

	[TestClass]
	public class ServerProjectionListOperationsTest : ProjectionListOperationsTest
	{
		public ServerProjectionListOperationsTest()
			: base(GetContext())
		{
		}

		public ServerProjectionListOperationsTest(ScenarioRunner runner)
			: base(GetContext(), runner)
		{
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}

	[TestClass]
	public class ServerSetListOperationsTest : SetListOperationsTest
	{
		public ServerSetListOperationsTest()
			: base(GetContext())
		{
		}

		public ServerSetListOperationsTest(ScenarioRunner runner)
			: base(GetContext(), runner)
		{
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}
}