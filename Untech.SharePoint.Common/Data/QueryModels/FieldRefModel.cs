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

			Type = FieldRefType.KnownMember;
			Member = member;
		}

		private FieldRefModel()
		{

		}

		public static FieldRefModel Key()
		{
			return new FieldRefModel
			{
				Type = FieldRefType.Key
			};
		}

		public FieldRefType Type { get; private set; }

		/// <summary>
		/// Gets <see cref="MemberInfo"/> that associated with current FieldRef.
		/// </summary>
		[CanBeNull]
		public MemberInfo Member { get; private set; }

		/// <summary>
		/// Returns a <see cref="String"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			if (Type == FieldRefType.Key)
			{
				return "<FieldRef Name='ID' />";
			}
			return Member != null ? string.Format("<FieldRef Name='{0}' />", Member.Name) : "<FieldRef Name='' />";
		}
	}
}