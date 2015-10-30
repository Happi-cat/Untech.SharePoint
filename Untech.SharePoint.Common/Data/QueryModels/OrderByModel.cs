using JetBrains.Annotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.QueryModels
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
			Guard.CheckNotNull("fieldRef", fieldRef);

			FieldRef = fieldRef;
			Ascending = ascending;
		}

		/// <summary>
		/// Determines whether ordering ascending or not.
		/// </summary>
		public bool Ascending { get; private set; }

		/// <summary>
		/// Gets associated FieldRef.
		/// </summary>
		[NotNull]
		public FieldRefModel FieldRef { get; private set; }

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
			return string.Format("<FieldRef Name='{0}' Ascending='{1}' />", FieldRef.Member.Name, Ascending.ToString().ToUpper());
		}
	}
}