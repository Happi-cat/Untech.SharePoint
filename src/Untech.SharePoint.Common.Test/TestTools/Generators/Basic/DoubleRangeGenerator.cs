namespace Untech.SharePoint.Common.TestTools.Generators.Basic
{
	public class DoubleRangeGenerator : BaseRandomGenerator, IValueGenerator<double>, IValueGenerator<double?>
	{
		public double Min { get; set; }

		public double Max { get; set; }

		public double Generate()
		{
			return Rand.NextDouble() * (Max - Min) + Min;
		}

		double? IValueGenerator<double?>.Generate()
		{
			return Generate();
		}
	}
}