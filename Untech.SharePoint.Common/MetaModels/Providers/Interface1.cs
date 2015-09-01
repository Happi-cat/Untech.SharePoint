using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Common.MetaModels.Providers
{
	public interface IMetaContextProvider
	{
		MetaContext GetMetaContext();
	}

	public interface IMetaListProvider
	{
		MetaList GetMetaList(MetaContext parent);
	}

	public interface IMetaContentTypeProvider
	{
		MetaContentType GetMetaContentType(MetaList parent);
	}

	public interface IMetaFieldProvider
	{
		MetaField GetMetaField(MetaContentType parent);
	}
}
