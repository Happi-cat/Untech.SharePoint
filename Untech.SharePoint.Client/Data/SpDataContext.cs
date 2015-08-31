using System;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Meta;

namespace Untech.SharePoint.Client.Data
{
	public abstract class SpDataContext<TDerived> : IDataContext
		where TDerived : SpDataContext<TDerived>
	{
		protected SpDataContext(ClientContext context)
		{

		}

		protected SpList<T> GetList<T>(Expression<Func<TDerived, SpList<T>>> listSelector)
		{
			var node = listSelector.Body.StripQuotes();
			if (node.NodeType == ExpressionType.MemberAccess)
			{
				var memberNode = (MemberExpression) node;
				var memberInfo = memberNode.Member;

				return GetList<T>(Mapping.GetList(memberInfo));
			}

			throw new ArgumentException("Selector is invalid", "listSelector");
		}

		private SpList<T> GetList<T>(MetaList metaList)
		{
			throw new NotImplementedException();
		}

		public ClientContext ClientContext
		{
			get { throw new NotImplementedException(); }
		}

		public MetaModel Mapping
		{
			get { throw new NotImplementedException(); }
		}
	}
}
