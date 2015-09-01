namespace Untech.SharePoint.Common.MetaModels.Providers
{
	public interface IMetaFieldProvider
	{
		MetaField GetMetaField(MetaContentType parent);
	}
}
