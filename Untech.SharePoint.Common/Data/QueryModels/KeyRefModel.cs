namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Represents FieldRef tag in CAML query that associated with key field, i.e. ID or BdcIdentity for external list.
	/// </summary>
	public sealed class KeyRefModel : FieldRefModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyRefModel"/>.
		/// </summary>
		public KeyRefModel() : 
			base(FieldRefType.Key)
		{
		}

		/// <summary>
		/// Returns a <see cref="string"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			return "<FieldRef Name='ID' />";
		}
	}
}