using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.TestTools.Comparers
{
	public class EntityComparer : IEqualityComparer<Entity>, IEqualityComparer<IEnumerable<Entity>>
	{
		public static readonly EntityComparer Default = new EntityComparer();

		public bool Equals(Entity x, Entity y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;

			return x.Id == y.Id && x.ContentTypeId == y.ContentTypeId;
		}

		public bool Equals(IEnumerable<Entity> x, IEnumerable<Entity> y)
		{
			if (object.Equals(x, y)) return true;
			if (x == null || y == null) return false;

			return x.SequenceEqual(y, this);
		}

		public int GetHashCode(Entity obj)
		{
			throw new NotImplementedException();
		}

		public int GetHashCode(IEnumerable<Entity> obj)
		{
			throw new NotImplementedException();
		}
	}
}