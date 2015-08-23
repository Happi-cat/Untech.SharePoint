using System;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	internal class SpClientMetaAccessor : MetaAccessor<ListItem>
	{
		public SpClientMetaAccessor(MetaDataMember member, Field field)
			: base(member)
		{
			SpField = field;
		}

		public Field SpField { get; private set; }

		public override object GetValue(ListItem instance)
		{
			if (!CanRead) throw new InvalidOperationException();

			return instance[DataMember.SpFieldInternalName];
		}

		public override void SetValue(ListItem instance, object value)
		{
			if (!CanWrite) throw new InvalidOperationException();

			instance[DataMember.SpFieldInternalName] = value;
		}

		public override bool CanWrite
		{
			get { return !SpField.ReadOnlyField; }
		}
	}
}