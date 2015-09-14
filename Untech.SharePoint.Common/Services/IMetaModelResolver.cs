using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
		IMetaContextProvider ContextProvider { get; }

		IListTitleResolver ListTitleResolver { get; }
	}
}
