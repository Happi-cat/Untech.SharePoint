using System;

namespace Untech.SharePoint.Core.Data.Fields
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SPFieldAttribute : Attribute
    {
        public SPFieldAttribute(string internalName)
        {
            InternalName = internalName;
        } 

        public string InternalName { get; private set; }

        public Type CustomConverterType { get; set; }
    }
}