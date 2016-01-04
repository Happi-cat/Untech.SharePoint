namespace Untech.SharePoint.Common.Test.Tools.Generators.Basic
{
	public class IntRangeGenerator : BaseRandomGenerator, IValueGenerator<int>, IValueGenerator<int?>
	{
		public int Min { get; set; }

		public int Max { get; set; }

		public int Generate()
		{
			return Rand.Next(Max - Min) + Min;
		}

		int? IValueGenerator<int?>.Generate()
		{
			return Generate();
		}
	}
}