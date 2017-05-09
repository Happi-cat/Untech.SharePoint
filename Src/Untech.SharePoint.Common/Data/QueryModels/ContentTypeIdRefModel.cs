namespace Untech.SharePoint.Data.QueryModels
{
	/// <summary>
	/// Represents FieldRef tag in CAML query that associated with ContentTypeId field.
	/// </summary>
	public sealed class ContentTypeIdRefModel : FieldRefModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTypeIdRefModel"/>.
		/// </summary>
		public ContentTypeIdRefModel() :
			base(FieldRefType.ContentTypeId)
		{
		}

		/// <summary>
		/// Returns a <see cref="string"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			return "<FieldRef Name='ContentTypeId' />";
		}
	}
}