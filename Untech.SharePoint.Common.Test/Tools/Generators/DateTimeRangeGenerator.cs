using System;

namespace Untech.SharePoint.Common.Test.Tools.Generators
{
	public class DateTimeRangeGenerator : BaseRandomGenerator, IValueGenerator<DateTime>, IValueGenerator<DateTime?>
	{
		public DateTimeRangeGenerator()
		{
			From = DateTime.Now.AddMonths(-1);
			To = DateTime.Now.AddMonths(+1);
		}

		public DateTime From{ get; set; }

		public DateTime To { get; set; }

		public DateTime Generate()
		{
			var ticksBetween = (To - From).Ticks;

			var ticks = (long) (Rand.NextDouble() * ticksBetween) + From.Ticks;

			return new DateTime(ticks);
		}

		DateTime? IValueGenerator<DateTime?>.Generate()
		{
			return Generate();
		}
	}
}