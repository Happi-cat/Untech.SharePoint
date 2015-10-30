using System;
using System.Reflection;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Represents FieldRef tag in CAML query.
	/// </summary>
	public sealed class FieldRefModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldRefModel"/> for the specified <see cref="MemberInfo"/>.
		/// </summary>
		/// <param name="member">Entity member</param>
		/// <exception cref="ArgumentNullException"><paramref name="member"/> is null.</exception>
		public FieldRefModel([NotNull]MemberInfo member)
		{
			Guard.CheckNotNull("member", member);

			Member = member;
		}

		/// <summary>
		/// Gets <see cref="MemberInfo"/> that associated with current FieldRef.
		/// </summary>
		[NotNull]
		public MemberInfo Member { get; private set; }

		/// <summary>
		/// Returns a <see cref="String"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			return string.Format("<FieldRef Name='{0}' />", Member.Name);
		}
	}
}