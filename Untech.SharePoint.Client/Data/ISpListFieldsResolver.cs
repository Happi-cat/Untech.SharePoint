using Untech.SharePoint.Client.Meta.Collections;

namespace Untech.SharePoint.Client.Data
{
	internal interface ISpFieldsResolver
	{
		SpFieldCollection GetFields(string listTitle);
	}
}