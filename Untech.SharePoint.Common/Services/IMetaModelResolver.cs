using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Services
{
	public interface IListTitleResolver
	{
		string GetListTitleFromContextProperty(PropertyInfo property);
	}

	public interface IMappingSource
	{
		Type ContextType { get; }

		IMetaContextProvider ContextProvider { get; }

		IListTitleResolver ListTitleResolver { get; }
	}

	public interface IMappingSource<TContext> : IMappingSource
		where TContext : ISpContext
	{
	}
}
