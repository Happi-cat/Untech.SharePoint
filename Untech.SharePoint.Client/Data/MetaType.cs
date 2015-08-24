using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaType
	{
		private readonly MetaModel _model;
		private readonly MetaList _list;
		private readonly Type _type;

		protected MetaType(MetaModel model, MetaList list, Type type)
		{
			Guard.CheckNotNull("model", model);
			Guard.CheckNotNull("list", list);
			Guard.CheckNotNull("type", type);

			_model = model;
			_list = list;
			_type = type;
		}

		public MetaModel Model { get { return _model; } }

		public MetaList List { get { return _list; } }

		public Type Type { get { return _type; } }

		public abstract DataMemberCollection DataMembers { get; }

		public override string ToString()
		{
			return string.Format("( Type={0}; Members=[{1}]; )", Type, DataMembers.JoinToString());
		}
	}
}

