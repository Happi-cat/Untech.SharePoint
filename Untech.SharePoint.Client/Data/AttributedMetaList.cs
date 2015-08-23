using System;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class AttributedMetaList : MetaList
	{

		public override MetaModel Model
		{
			get { throw new NotImplementedException(); }
		}

		public override string ListTitle
		{
			get { throw new NotImplementedException(); }
		}

		public override SpFieldCollection Fields { get { throw new NotImplementedException();} }


		public override MetaType ItemType
		{
			get { throw new NotImplementedException(); }
		}
	}
}