using System;

namespace Untech.SharePoint.Data.Fields
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SPFieldAttribute : Attribute
    {
        public SPFieldAttribute(string internalName)
        {
            InternalName = internalName;
        } 

        public string InternalName { get; private set; }

        public Type ConverterType { get; set; }
    }
}