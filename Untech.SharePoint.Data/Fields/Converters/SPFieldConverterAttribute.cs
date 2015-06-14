using System;

namespace Untech.SharePoint.Data.Fields.Converters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SPFieldConverterAttribute : Attribute
    {
        public SPFieldConverterAttribute(string fieldType)
        {
            FieldType = fieldType;
        }

        public string FieldType { get; private set; }
    }
}