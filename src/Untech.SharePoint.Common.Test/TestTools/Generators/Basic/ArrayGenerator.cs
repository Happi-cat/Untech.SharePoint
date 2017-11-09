using System.Collections.Generic;

namespace Untech.SharePoint.TestTools.Generators.Basic
{
	public class ArrayGenerator<T> : BaseRandomGenerator, IValueGenerator<List<T>>, IValueGenerator<T[]>
	{
		public ArrayGenerator(IValueGenerator<T> itemGenerator)
		{
			ItemGenerator = itemGenerator;
		}

		public int Size { get; set; }

		public ArrayGenerationOptions Options { get; set; }

		public IValueGenerator<T> ItemGenerator { get; }

		public virtual List<T> Generate()
		{
			var counter = GetSize();

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

		private int GetSize()
		{
			return Options == ArrayGenerationOptions.RandomSize ? Rand.Next(Size) : Size;
		}
	}
}