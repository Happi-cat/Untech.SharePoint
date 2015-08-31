using System;
using Untech.SharePoint.Client.Utils.Reflection;

namespace Untech.SharePoint.Client.Meta.Accessors
{
	internal class MemberMetaAccessor : MetaAccessor<object>
	{
		public MemberMetaAccessor(MetaDataMember member, MemberAccessor accessor)
			: base(member)
		{
			Accessor = accessor;
		}

		public MemberAccessor Accessor { get; private set; }

		public override object GetValue(object instance)
		{
			if (!CanRead) throw new InvalidOperationException();

			return Accessor[instance, DataMember.Name];
		}

		public override void SetValue(object instance, object value)
		{
			if (!CanWrite) throw new InvalidOperationException();

			Accessor[instance, DataMember.Name] = value;
		}

		public override bool CanRead
		{
			get { return Accessor.CanRead(DataMember.Name); }
		}

		public override bool CanWrite
		{
			get { return Accessor.CanWrite(DataMember.Name); }
		}
	}
}