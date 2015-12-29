using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Test.Tools.Generators
{
	public static class ItemFillerExtensions
	{
		public static ItemFiller<T> WithStatic<T, TProp>(this ItemFiller<T> filler, Expression<Func<T, TProp>> selector, TProp value)
		{
			return filler.With(selector, new StaticGenerator<TProp>(value));
		}

		public static ItemFiller<T> WithArray<T, TProp>(this ItemFiller<T> filler, Expression<Func<T, TProp[]>> selector,
			int size, IEnumerable<TProp> range)
		{
			return filler.WithArray(selector, size, new RangeGenerator<TProp>(range));
		}

		public static ItemFiller<T> WithArray<T, TProp>(this ItemFiller<T> filler, Expression<Func<T, TProp[]>> selector,
			int size, IValueGenerator<TProp> itemGenerator)
		{
			return filler.With(selector, new ArrayGenerator<TProp>(itemGenerator) { Size = size });
		}

		public static ItemFiller<T> WithArray<T, TProp>(this ItemFiller<T> filler,
			Expression<Func<T, IEnumerable<TProp>>> selector, int size, IEnumerable<TProp> range)
		{
			return filler.WithArray(selector, size, new RangeGenerator<TProp>(range));
		}

		public static ItemFiller<T> WithArray<T, TProp>(this ItemFiller<T> filler,
			Expression<Func<T, IEnumerable<TProp>>> selector, int size, IValueGenerator<TProp> itemGenerator)
		{
			return filler.With(selector, new ArrayGenerator<TProp>(itemGenerator) { Size = size });
		}

		public static ItemFiller<T> WithRange<T, TProp>(this ItemFiller<T> filler, Expression<Func<T, TProp>> selector,
			IEnumerable<TProp> range) where T : new()
		{
			return filler.With(selector, new RangeGenerator<TProp>(range));
		}

		public static ItemFiller<T> WithShortLorem<T>(this ItemFiller<T> filler, Expression<Func<T, string>> selector)

		{
			return filler.With(selector, new LoremGenerator
			{
				MinWords = 5,
				MaxWords = 10
			});
		}

		public static ItemFiller<T> WithLongLorem<T>(this ItemFiller<T> filler, Expression<Func<T, string>> selector)

		{
			return filler.With(selector, new LoremGenerator
			{
				MinWords = 5,
				MaxWords = 10,
				MinSentences = 2,
				MaxSentences = 5,
				ParagraphsNumber = 5
			});
		}


		public static ItemFiller<T> WithHtmlLongLorem<T>(this ItemFiller<T> filler, Expression<Func<T, string>> selector)

		{
			return filler.With(selector, new LoremGenerator
			{
				MinWords = 5,
				MaxWords = 10,
				MinSentences = 2,
				MaxSentences = 5,
				ParagraphsNumber = 5,
				Html = true
			});
		}

		public static ItemFiller<T> WithPastDate<T>(this ItemFiller<T> filler, Expression<Func<T, DateTime?>> selector)

		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-12),
				To = DateTime.Now.AddMonths(-1)
			});
		}

		public static ItemFiller<T> WithPastDate<T>(this ItemFiller<T> filler, Expression<Func<T, DateTime>> selector)

		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-12),
				To = DateTime.Now.AddMonths(-1)
			});
		}

		public static ItemFiller<T> WithFutureDate<T>(this ItemFiller<T> filler,
			Expression<Func<T, DateTime?>> selector)

		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(+1),
				To = DateTime.Now.AddMonths(+12)
			});
		}

		public static ItemFiller<T> WithFutureDate<T>(this ItemFiller<T> filler,
			Expression<Func<T, DateTime>> selector)

		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(+1),
				To = DateTime.Now.AddMonths(+12)
			});
		}

		public static ItemFiller<T> WithActualDate<T>(this ItemFiller<T> filler, Expression<Func<T, DateTime?>> selector)

		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-1),
				To = DateTime.Now.AddDays(+1)
			});
		}

		public static ItemFiller<T> WithActualDate<T>(this ItemFiller<T> filler, Expression<Func<T, DateTime>> selector)

		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-1),
				To = DateTime.Now.AddDays(+1)
			});
		}

		
	}
}