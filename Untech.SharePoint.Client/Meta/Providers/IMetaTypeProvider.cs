namespace Untech.SharePoint.Client.Meta.Providers
{
	public interface IMetaTypeProvider
	{
		MetaType GetMetaType(MetaList metaList);
	}
}