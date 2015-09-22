using System.Reflection;

namespace Untech.SharePoint.Common.Mappings
{
	public interface IListTitleResolver
	{
		string GetListTitleFromContextProperty(PropertyInfo property);
	}
}