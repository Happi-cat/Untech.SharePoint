using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class EntityComparer : IEqualityComparer<Entity>
	{
		public static readonly IEqualityComparer<Entity> Default = new EntityComparer();

		public bool Equals(Entity x, Entity y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;

			return x.Id == y.Id && x.ContentTypeId == y.ContentTypeId;
		}

		public int GetHashCode(Entity obj)
		{
			if (obj == null) return 0;
			var hash = 0;
			hash ^= obj.Id;
			hash ^= (obj.ContentTypeId ?? "").GetHashCode();

			return hash;
		}
	}

	public class EntitySequenceComparer<T> :IEqualityComparer<IEnumerable<T>>
		where T : Entity
	{
		public static readonly IEqualityComparer<IEnumerable<T>> Default = new EntitySequenceComparer<T>();

		public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
		{
			if (object.Equals(x, y)) return true;
			if (x == null || y == null) return false;

			return x.SequenceEqual(y, EntityComparer.Default);
		}

		public int GetHashCode(IEnumerable<T> obj)
		{
			throw new System.NotImplementedException();
		}
	}

	public class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>>
	{
		public static readonly IEqualityComparer<IEnumerable<T>> Default = new SequenceComparer<T>();

		public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
		{
			if (object.Equals(x, y)) return true;
			if (x == null || y == null) return false;

			return x.SequenceEqual(y);
		}

		public int GetHashCode(IEnumerable<T> obj)
		{
			throw new System.NotImplementedException();
		}
	}
}