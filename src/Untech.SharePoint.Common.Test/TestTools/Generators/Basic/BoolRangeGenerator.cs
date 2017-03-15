namespace Untech.SharePoint.Common.TestTools.Generators.Basic
{
	public class BoolGenerator : BaseRandomGenerator, IValueGenerator<bool>, IValueGenerator<bool?>
	{
		public bool Generate()
		{
			return Rand.Next(100) > 50;
		}

		bool? IValueGenerator<bool?>.Generate()
		{
			return Generate();
		}
	}
}