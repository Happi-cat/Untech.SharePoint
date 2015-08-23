using System.Linq;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class TypeMapper
	{
		public TypeMapper(MetaType type)
		{
			Guard.CheckNotNull("type", type);

			Type = type;
		}

		public MetaType Type { get; private set; }

		public void Map(object source, ListItem dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			Type.DataMembers
				.Select(n => new DataMemberMapper(n))
				.ToList()
				.ForEach(n => n.Map(source, dest));
		}

		public void Map(ListItem source, object dest)
		{
			Guard.CheckNotNull("source", source);
			Guard.CheckNotNull("dest", dest);

			Type.DataMembers
				.Select(n => new DataMemberMapper(n))
				.ToList()
				.ForEach(n => n.Map(source, dest));
		}
	}
}