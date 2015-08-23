using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Client.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaType
	{
		public abstract MetaModel Model { get; }

		public abstract MetaList List { get; }

		public abstract Type Type { get; }

		public abstract DataMemberCollection DataMembers { get; }
	}
}

