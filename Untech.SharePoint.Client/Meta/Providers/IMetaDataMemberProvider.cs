namespace Untech.SharePoint.Client.Meta.Providers
{
	public interface IMetaDataMemberProvider
	{
		MetaDataMember GetMetaDataMember(MetaType metaType);
	}
}