using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	public interface IMetaContextProcessor
	{
		void Process(MetaContext context);
	}
}