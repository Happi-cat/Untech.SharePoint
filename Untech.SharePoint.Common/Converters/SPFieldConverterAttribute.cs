using System;

namespace Untech.SharePoint.Common.Converters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class SpFieldConverterAttribute : Attribute
	{
		public SpFieldConverterAttribute(string fieldType)
		{
			FieldTypeAsString = fieldType;
		}

		public string FieldTypeAsString { get; private set; }
	}
}