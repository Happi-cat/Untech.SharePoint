using System;
using System.Reflection;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Represents FieldRef tag in CAML query.
	/// </summary>
	public abstract class FieldRefModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldRefModel"/> with the specified <see cref="FieldRefType"/>.
		/// </summary>
		/// <param name="fieldRefType">FieldRef type</param>
		protected internal FieldRefModel(FieldRefType fieldRefType)
		{
			Type = fieldRefType;
		}

		/// <summary>
		/// Gets current field ref type.
		/// </summary>
		public FieldRefType Type { get; private set; }

		/// <summary>
		/// Returns a <see cref="String"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			return "<InvalidFieldRef Name='' />";
		}
	}
}