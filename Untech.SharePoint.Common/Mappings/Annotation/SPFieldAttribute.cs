using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	/// <summary>
	/// When applied to property or field, specifies member that should be mapped to existing SP Field.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[PublicAPI]
	public sealed class SpFieldAttribute : Attribute
	{
		/// <summary>
		/// Initializes new instance of the <see cref="SpFieldAttribute"/>
		/// </summary>
		public SpFieldAttribute() { }

		/// <summary>
		/// Initializes new instance of the <see cref="SpFieldAttribute"/>
		/// </summary>
		public SpFieldAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Gets or sets SP field InternalName.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets SP field type.
		/// </summary>
		public string FieldType { get; set; }

		/// <summary>
		/// Gets or sets SP field custom <see cref="IFieldConverter"/> converter.
		/// </summary>
		public Type CustomConverterType { get; set; }
	}
}