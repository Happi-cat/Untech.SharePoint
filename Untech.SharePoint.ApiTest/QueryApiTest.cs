using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.ApiTest.Models;
using Untech.SharePoint.Client.Configuration;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Server.Configuration;

namespace Untech.SharePoint.ApiTest
{
	[TestClass]
	public class QueryApiTest
	{
		[TestMethod]
		public void GetById()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Get(1), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void SimpleQuery()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x, new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void WhereQuery()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Where(n => n.Title.Contains("T")), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void WhereWithEnumsQuery()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Where(n => n.Status == ProjectStatus.Completed), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void TakeQuery()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Take(10), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void LastQuery()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Last(), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void FirstQuery()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.First(), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void OrderByQuery()
		{
			Test(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.OrderByDescending(n=> n.Modified), new DevelopmentProjectComparer());
		}

		public Config BuildConfig(ConfigBuilder builder)
		{
			return builder
				.RegisterMappings(n => n.Annotated<ServerDataContext>())
				.RegisterMappings(n => n.Annotated<ClientDataContext>())
				.BuildConfig();
		}

		public ServerDataContext GetServerCtx()
		{
			var cfg = BuildConfig(ServerConfig.Begin());
			var site = new SPSite("http://spnthpdvds0040v:8086/sites/investment");
			return new ServerDataContext(site.OpenWeb(), cfg);
		}

		public ClientDataContext GetClientCtx()
		{
			var cfg = BuildConfig(ClientConfig.Begin());
			var ctx = new ClientContext("http://spnthpdvds0040v:8086/sites/investment");
			return new ClientDataContext(ctx, cfg);
		}

		public void Test<T1, T2>(Func<ServerDataContext, ISpList<T1>> serverListSelector,
			Func<ClientDataContext, ISpList<T1>> clientListSelector, Func<ISpList<T1>, IQueryable<T2>> query,
			IEqualityComparer<T2> comparer)
		{
			var serverList = serverListSelector(GetServerCtx());
			var clientList = clientListSelector(GetClientCtx());

			var serverQueryResult = query(serverList).ToList();
			var clientQueryResult = query(clientList).ToList();

			Assert.IsTrue(serverQueryResult.SequenceEqual(clientQueryResult, comparer));
		}

		public void Test<T1, T2>(Func<ServerDataContext, ISpList<T1>> serverListSelector,
			Func<ClientDataContext, ISpList<T1>> clientListSelector, Func<ISpList<T1>, T2> query,
			IEqualityComparer<T2> comparer)
		{
			var serverList = serverListSelector(GetServerCtx());
			var clientList = clientListSelector(GetClientCtx());

			var serverQueryResult = query(serverList);
			var clientQueryResult = query(clientList);

			Assert.IsTrue(comparer.Equals(serverQueryResult, clientQueryResult));
		}

		public class DevelopmentProjectComparer : IEqualityComparer<DevelopmentProject>
		{
			public bool Equals(DevelopmentProject x, DevelopmentProject y)
			{
				return Check(x, y, n => n.Id) &&
				       Check(x, y, n => n.Title) &&
				       Check(x, y, n => n.Created) &&
				       Check(x, y, n => n.Modified) &&
				       Check(x, y, n => n.ContentTypeId) &&
				       Check(x, y, n => n.Status) &&
				       Check(x, y, n => n.Gate) &&
				       Check(x, y, n => n.ProjectUid) &&
				       Check(x, y, n => n.ProjectNo) &&
				       Check(x, y, n => n.LobBudgetOwner) &&
				       Check(x, y, n => n.LobDeliveryOwner) &&
				       Check(x, y, n => n.SponsoringPortfolio) &&
				       Check(x, y, n => n.ParentPortfolio) &&
				       Check(x, y, n => n.ChildPortfolio) &&
				       Check(x, y, n => n.Methodology) &&
				       Check(x, y, n => n.Status) &&
				       Check(x, y, n => n.Approver) &&
				       Check(x, y, n => n.DecisionDate) &&
				       Check(x, y, n => n.ProjectComments);

			}

			public int GetHashCode(DevelopmentProject obj)
			{
				if (obj == null) return 0;
				return obj.GetHashCode();
			}

			protected bool Check<TProp>(DevelopmentProject x, DevelopmentProject y, Func<DevelopmentProject, TProp> selector)
			{
				return EqualityComparer<TProp>.Default.Equals(selector(x), selector(y));
			}

			protected bool Check(DevelopmentProject x, DevelopmentProject y, Func<DevelopmentProject, UserInfo> selector)
			{
				var xUser = selector(x);
				var yUser = selector(y);

				if (xUser == yUser) return true;
				if (xUser == null || yUser == null) return false;
				return xUser.Id == yUser.Id;
			}

			protected bool Check(DevelopmentProject x, DevelopmentProject y, Func<DevelopmentProject, ObjectReference> selector)
			{
				var xRef = selector(x);
				var yRef = selector(y);

				if (xRef == yRef) return true;
				if (xRef == null || yRef == null) return false;
				return xRef.Id == yRef.Id;
			}
		}
	}

}
