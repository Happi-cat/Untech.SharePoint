using System;

namespace Untech.SharePoint.Core.Data.Converters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class SpFieldConverterAttribute : Attribute
	{
		public SpFieldConverterAttribute(string fieldType)
		{
			FieldType = fieldType;
		}

		public string FieldType { get; private set; }
	}
}