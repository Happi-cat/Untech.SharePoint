using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.ApiTest.Models;
using Untech.SharePoint.Client.Configuration;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Server.Configuration;

namespace Untech.SharePoint.ApiTest
{
	[TestClass]
	public class QueryApiTest
	{
		[TestMethod]
		public void SimpleQuery()
		{
			Equals(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x, new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void WhereQuery()
		{
			Equals(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Where(n => n.Title.Contains("T")), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void TakeQuery()
		{
			Equals(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Take(10), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void LastQuery()
		{
			Equals(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.Last(), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void FirstQuery()
		{
			Equals(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
				x => x.First(), new DevelopmentProjectComparer());
		}

		[TestMethod]
		public void OrderByQuery()
		{
			Equals(x => x.DevelopmentProjects, x => x.DevelopmentProjects,
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
			throw new NotImplementedException();
		}

		public ClientDataContext GetClientCtx()
		{
			var cfg = BuildConfig(ClientConfig.Begin());
			throw new NotImplementedException();
		}

		public void Equals<T1, T2>(Func<ServerDataContext, ISpList<T1>> serverListSelector,
			Func<ClientDataContext, ISpList<T1>> clientListSelector, Func<ISpList<T1>, IQueryable<T2>> query,
			IEqualityComparer<T2> comparer)
		{
			var serverList = serverListSelector(GetServerCtx());
			var clientList = clientListSelector(GetClientCtx());

			var serverQueryResult = query(serverList).ToList();
			var clientQueryResult = query(clientList).ToList();

			Assert.IsTrue(serverQueryResult.SequenceEqual(clientQueryResult, comparer));
		}

		public void Equals<T1, T2>(Func<ServerDataContext, ISpList<T1>> serverListSelector,
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
				throw new NotImplementedException();
			}

			public int GetHashCode(DevelopmentProject obj)
			{
				throw new NotImplementedException();
			}
		}
	}
}
