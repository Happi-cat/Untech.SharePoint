using System.Reflection;

namespace Untech.SharePoint.Mappings
{
	/// <summary>
	/// Represents interface of object that can resolve list URL by context member.
	/// </summary>
	public interface IListUrlResolver
	{
		/// <summary>
		/// Gets list URL that associated with <paramref name="member"/>.
		/// </summary>
		/// <param name="member">Member to resolve.</param>
		/// <returns>The site-relative URL at which the list was placed that associated with <paramref name="member"/>.</returns>
		string GetListUrlFromContextMember(MemberInfo member);
	}
}