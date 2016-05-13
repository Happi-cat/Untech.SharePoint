using System;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Data
{
	/// <summary>
	/// Represents base data context class for CSOM.
	/// </summary>
	/// <typeparam name="TContext">Type of the data context.</typeparam>
	public abstract class SpClientContext<TContext> : SpContext<TContext>
		where TContext : SpClientContext<TContext>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpClientContext{TContext}"/> with specified <see cref="ClientContext"/> and <see cref="Config"/>.
		/// </summary>
		/// <param name="context">Client content to use for data access.</param>
		/// <param name="config">Configuration.</param>
		/// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="config"/> is null.</exception>
		protected SpClientContext([NotNull] ClientContext context, [NotNull] Config config)
			: base(new SpClientCommonService(context, config))
		{
			Guard.CheckNotNull("context", context);

			ClientContext = context;
		}

		/// <summary>
		/// Gets <see cref="ClientContext"/> that is associated with the current data context.
		/// </summary>
		public ClientContext ClientContext { get; private set; }

		/// <summary>
		/// Gets <see cref="List"/> instance by list accessor.
		/// </summary>
		/// <typeparam name="TEntity">Type of element.</typeparam>
		/// <param name="listSelector">List property accessor.</param>
		/// <returns>Instance of the <see cref="List"/>.</returns>
		public List GetSPList<TEntity>(Expression<Func<TContext, ISpList<TEntity>>> listSelector)
		{
			return ClientContext.GetList(GetListTitle(listSelector));
		}
	}
}
