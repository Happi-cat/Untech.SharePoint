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
			Test(x => x.News, x => x.News,
				x => x.Get(3), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void SimpleQuery()
		{
			Test(x => x.News, x => x.News,
				x => x, new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void WhereQuery()
		{
			Test(x => x.News, x => x.News,
				x => x.Where(n => n.Title.Contains("T")), new DevelopmentProjectComparer());
		}

	[TestMethod]
		public void WhereWithDateTimeQuery()
		{
			Test(x => x.News, x => x.News,
				x => x.Where(n => n.Created > DateTime.Now.AddMonths(-1)), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void WhereWithUserQuery()
		{
			// TODO: wrong result
			var me = new UserInfo { Id = 1 };
			Test(x => x.News, x => x.News,
				x => x.Where(n => n.Author == me), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void TakeQuery()
		{
			Test(x => x.News, x => x.News,
				x => x.Take(10), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void LastQuery()
		{
			Test(x => x.News, x => x.News,
				x => x.Last(), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void FirstQuery()
		{
			Test(x => x.News, x => x.News,
				x => x.First(), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void OrderByQuery()
		{
			Test(x => x.News, x => x.News,
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
			var site = new SPSite("http://sp2013dev/sites/orm-test");
			return new ServerDataContext(site.OpenWeb(), cfg);
		}

		public ClientDataContext GetClientCtx()
		{
			var cfg = BuildConfig(ClientConfig.Begin());
			var ctx = new ClientContext("http://sp2013dev/sites/orm-test");
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

		public class DevelopmentProjectComparer : IEqualityComparer<NewsItem>
		{
			public bool Equals(NewsItem x, NewsItem y)
			{
				return Check(x, y, n => n.Id) &&
				       Check(x, y, n => n.Title) &&
				       //sCheck(x, y, n => n.Created) &&
				       //Check(x, y, n => n.Modified) &&
				       Check(x, y, n => n.ContentTypeId);

			}

			public int GetHashCode(NewsItem obj)
			{
				if (obj == null) return 0;
				return obj.GetHashCode();
			}

			protected bool Check<TProp>(NewsItem x, NewsItem y, Func<NewsItem, TProp> selector)
			{
				return EqualityComparer<TProp>.Default.Equals(selector(x), selector(y));
			}

			protected bool Check(NewsItem x, NewsItem y, Func<NewsItem, UserInfo> selector)
			{
				var xUser = selector(x);
				var yUser = selector(y);

				if (xUser == yUser) return true;
				if (xUser == null || yUser == null) return false;
				return xUser.Id == yUser.Id;
			}

			protected bool Check(NewsItem x, NewsItem y, Func<NewsItem, ObjectReference> selector)
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
