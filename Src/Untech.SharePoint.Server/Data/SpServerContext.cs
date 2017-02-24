using System;
using System.Linq.Expressions;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
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
			: base(new SpServerCommonService(web, config))
		{
			Guard.CheckNotNull(nameof(web), web);

			Web = web;
		}

		/// <summary>
		/// Gets <see cref="SPWeb"/> that is associated with the current data context.
		/// </summary>
		public SPWeb Web { get; }

		/// <summary>
		/// Gets <see cref="SPList"/> instance by list accessor.
		/// </summary>
		/// <typeparam name="TEntity">Type of element.</typeparam>
		/// <param name="listSelector">List property accessor.</param>
		/// <returns>Instance of the <see cref="SPList"/>.</returns>
		public SPList GetSPList<TEntity>(Expression<Func<TContext, ISpList<TEntity>>> listSelector)
		{
			return Web.GetList(GetListUrl(listSelector));
		}
	}
}