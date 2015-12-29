using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Common.Test.Tools.Generators
{
	public class ArrayGenerator<T> : IValueGenerator<IEnumerable<T>>, IValueGenerator<T[]>
	{
		public ArrayGenerator(IValueGenerator<T> itemGenerator)
		{
			ItemGenerator = itemGenerator;
		}

		public int Size { get; set; }

		public IValueGenerator<T> ItemGenerator { get; private set; }

		public virtual IEnumerable<T> Generate()
		{
			var counter = Size;
			var list = new List<T>();
			while (counter-- > 0)
			{
				list.Add(ItemGenerator.Generate());
			}
			return list;
		}

		T[] IValueGenerator<T[]>.Generate()
		{
			return Generate().ToArray();
		}
	}
}