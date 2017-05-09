using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Data.QueryModels
{
	/// <summary>
	/// Represents CAML OrderBy tag.
	/// </summary>
	public sealed class OrderByModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrderByModel"/> for the specified <see cref="FieldRefModel"/>.
		/// </summary>
		/// <param name="fieldRef">FieldRef to order.</param>
		/// <param name="ascending">Is ascending ordering.</param>
		public OrderByModel([NotNull]FieldRefModel fieldRef, bool ascending)
		{
			Guard.CheckNotNull(nameof(fieldRef), fieldRef);

			FieldRef = fieldRef;
			Ascending = ascending;
		}

		/// <summary>
		/// Determines whether ordering ascending or not.
		/// </summary>
		public bool Ascending { get; }

		/// <summary>
		/// Gets associated FieldRef.
		/// </summary>
		[NotNull]
		public FieldRefModel FieldRef { get; }

		/// <summary>
		/// Returns reversed ordering.
		/// </summary>
		/// <returns>Reversed ordering.</returns>
		[NotNull]
		public OrderByModel Reverse()
		{
			return new OrderByModel(FieldRef, !Ascending);
		}

		/// <summary>
		/// Returns a <see cref="string"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			switch (FieldRef.Type)
			{
				case FieldRefType.Key:
					return $"<FieldRef Name='ID' Ascending='{Ascending.ToString().ToUpper()}' />";
				case FieldRefType.ContentTypeId:
					return $"<FieldRef Name='ContentTypeId' Ascending='{Ascending.ToString().ToUpper()}' />";
				case FieldRefType.KnownMember:
					var memberRef = (MemberRefModel)FieldRef;
					return $"<FieldRef Name='{memberRef.Member.Name}' Ascending='{Ascending.ToString().ToUpper()}' />";
			}
			return $"<InvalidFieldRef Name='' Ascending='{Ascending.ToString().ToUpper()}' />";
		}
	}
}