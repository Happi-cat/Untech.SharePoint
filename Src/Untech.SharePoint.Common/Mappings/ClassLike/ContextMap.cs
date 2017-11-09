using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.MetaModels.Providers;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Mappings.ClassLike
{
	/// <summary>
	/// Represents provider of <see cref="MetaContext"/> that allows to configure mappings of all defined lists in fluent way.
	/// </summary>
	public class ContextMap<TContext> : IMetaContextProvider, IListUrlResolver
		where TContext : ISpContext
	{
		private readonly Dictionary<string, ListPart<TContext>> _listParts;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContextMap{TEntity}"/>.
		/// </summary>
		public ContextMap()
		{
			_listParts = new Dictionary<string, ListPart<TContext>>(SiteRelativeUrlComparer.Default);
			MemberToListMap = new Dictionary<MemberInfo, ListPart<TContext>>(MemberInfoComparer.Default);
		}

		internal Dictionary<MemberInfo, ListPart<TContext>> MemberToListMap { get; }

		/// <summary>
		/// Initializes a new or returns existing instance of the <see cref="ListPart{TContext}"/> for list mapping configuration.
		/// </summary>
		/// <param name="url">Site-relative URL of the desirable list.</param>
		/// <returns>Instance of the <see cref="ListPart{TContext}"/> that allows to configure list mapping.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="url"/> is null or empty.</exception>
		[NotNull]
		public ListPart<TContext> List([NotNull]string url)
		{
			Guard.CheckNotNullOrEmpty(nameof(url), url);

			if (!_listParts.ContainsKey(url))
			{
				_listParts.Add(url, new ListPart<TContext>(this, url));
			}

			return _listParts[url];
		}

		MetaContext IMetaContextProvider.GetMetaContext()
		{
			return new MetaContext(_listParts.Select(n => n.Value).ToList());
		}

		string IListUrlResolver.GetListUrlFromContextMember(MemberInfo member)
		{
			return MemberToListMap[member].ListUrl;
		}
	}
}