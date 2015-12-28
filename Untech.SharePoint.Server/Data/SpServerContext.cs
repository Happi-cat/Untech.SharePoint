using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Data
{
	/// <summary>
	/// Represents base data context class for SSOM.
	/// </summary>
	/// <typeparam name="TContext">Type of the data context.</typeparam>
	public abstract class SpServerContext<TContext> : SpContext<TContext>
		where TContext : SpServerContext<TContext>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpServerContext{TContext}"/> with specified <see cref="SPWeb"/> and <see cref="Config"/>.
		/// </summary>
		/// <param name="web">SPWeb to use for data access.</param>
		/// <param name="config">Configuration.</param>
		/// <exception cref="ArgumentNullException"><paramref name="web"/> or <paramref name="config"/> is null.</exception>
		protected SpServerContext([NotNull] SPWeb web, [NotNull] Config config) 
			: base(config, new SpCommonService(web, config))
		{
			Guard.CheckNotNull("web", web);

			Web = web;
		}

		/// <summary>
		/// Gets <see cref="SPWeb"/> that is associated with the current data context.
		/// </summary>
		public SPWeb Web { get; private set; }

		protected override ISpListItemsProvider GetItemsProvider(MetaList list)
		{
			return new SpListItemsProvider(Web, list);
		}
	}
}