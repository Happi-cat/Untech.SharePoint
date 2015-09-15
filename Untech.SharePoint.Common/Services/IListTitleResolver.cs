using System.Reflection;

namespace Untech.SharePoint.Common.Services
{
	public interface IListTitleResolver
	{
		string GetListTitleFromContextProperty(PropertyInfo property);
	}
}