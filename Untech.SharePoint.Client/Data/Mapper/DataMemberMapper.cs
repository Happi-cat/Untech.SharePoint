using System;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.FieldConverters;
using Untech.SharePoint.Client.Meta;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class DataMemberMapper
	{
		public DataMemberMapper(MetaDataMember member)
		{
			Guard.CheckNotNull("member", member);

			DataMember = member;
		}

		public MetaDataMember DataMember { get; private set; }

		public MetaAccessor<object> MemberAccessor
		{
			get { return DataMember.MemberAccessor; }
		}

		public MetaAccessor<ListItem> SpClientAccessor
		{
			get { return DataMember.SpClientAccessor; }
		}

		public IFieldConverter Converter
		{
			get { return DataMember.Converter; }
		}

		public void Map(object source, ListItem dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			try
			{
				if (!MemberAccessor.CanRead || !SpClientAccessor.CanWrite)
					return;

				var clrValue = MemberAccessor.GetValue(source);
				var clientValue = Converter.ToSpClientValue(clrValue);
				SpClientAccessor.SetValue(dest, clientValue);
			}
			catch (Exception e)
			{
				throw new MemberMappingException(DataMember, e);
			}
		}

		public void Map(ListItem source, object dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			try
			{
				if (!SpClientAccessor.CanRead || !MemberAccessor.CanWrite) 
					return;

				var clientValue = SpClientAccessor.GetValue(source);
				var clrValue = Converter.FromSpClientValue(clientValue);
				MemberAccessor.SetValue(dest, clrValue);
			}
			catch (Exception e)
			{
				throw new MemberMappingException(DataMember, e);
			}
		}
	}
}