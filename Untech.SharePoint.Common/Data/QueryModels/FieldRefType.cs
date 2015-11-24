using System.Reflection;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Describes differenet types of <see cref="FieldRefModel"/>.
	/// </summary>
	public enum FieldRefType
	{
		/// <summary>
		/// FieldRef associated with the existing <see cref="MemberInfo"/>.
		/// </summary>
		KnownMember,
		/// <summary>
		/// FieldRef associated with key field, i.e. ID or BdcIdentity for external lists.
		/// </summary>
		Key,
		/// <summary>
		/// FieldRef associated with ContentTypeId field.
		/// </summary>
		ContentTypeId
	}
}