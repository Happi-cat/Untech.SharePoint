using System.Reflection;

namespace Untech.SharePoint.Common.Mappings
{
	/// <summary>
	/// Represents interface of object that can resolve list title by context member.
	/// </summary>
	public interface IListTitleResolver
	{
		/// <summary>
		/// Gets list title that associated with <paramref name="member"/>.
		/// </summary>
		/// <param name="member">Member to resolve.</param>
		/// <returns>List title that associated with <paramref name="member"/>.</returns>
		string GetListTitleFromContextMember(MemberInfo member);
	}
}