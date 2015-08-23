using System;
using System.Reflection;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.FieldConverters;

namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaDataMember
	{
		public abstract MetaType DeclaringType { get; }

		public abstract MemberInfo Member { get; }

		public abstract string Name { get; }

		public abstract Type Type { get; }

		public abstract string SpFieldInternalName { get; }

		public abstract string SpFieldTypeAsString { get; }

		public abstract IFieldConverter Converter { get; }

		public abstract MetaAccessor<object> MemberAccessor { get; }

		public abstract MetaAccessor<ListItem> SpClientAccessor { get; }

		public override string ToString()
		{
			return string.Format("(Name={0}; SpFieldInternalName={1})", Name, SpFieldInternalName);
		}
	}
}