using System;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Converters
{
	/// <summary>
	/// Specifies SP field type that can be converted by marked <see cref="IFieldConverter"/> class.
	/// </summary>
	[BaseTypeRequired(typeof(IFieldConverter))]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class SpFieldConverterAttribute : Attribute
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="SpFieldConverterAttribute"/>.
		/// </summary>
		/// <param name="fieldType">SP field type.</param>
		public SpFieldConverterAttribute(string fieldType)
		{
			FieldTypeAsString = fieldType;
		}

		/// <summary>
		/// Gets SP field type.
		/// </summary>
		public string FieldTypeAsString { get; private set; }
	}
}