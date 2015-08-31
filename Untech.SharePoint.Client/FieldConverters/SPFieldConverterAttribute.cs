using System;

namespace Untech.SharePoint.Client.FieldConverters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class SpFieldConverterAttribute : Attribute
	{
		public SpFieldConverterAttribute(string fieldType)
		{
			FieldTypeAsString = fieldType;
		}

		public string FieldTypeAsString { get; private set; }
	}
}