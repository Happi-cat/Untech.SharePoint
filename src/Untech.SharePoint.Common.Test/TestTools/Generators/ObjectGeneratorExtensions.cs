using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.TestTools.Generators.Basic;

namespace Untech.SharePoint.TestTools.Generators
{
	public static class ObjectGeneratorExtensions
	{
		public static ObjectGenerator<T> WithStatic<T, TProp>(this ObjectGenerator<T> filler, Expression<Func<T, TProp>> selector, TProp value)
		{
			return filler.With(selector, new StaticGenerator<TProp>(value));
		}

		public static ObjectGenerator<T> WithArray<T, TProp>(this ObjectGenerator<T> filler, Expression<Func<T, TProp[]>> selector,
			int size, IEnumerable<TProp> range)
		{
			return filler.WithArray(selector, size, new RangeGenerator<TProp>(range));
		}

		public static ObjectGenerator<T> WithArray<T, TProp>(this ObjectGenerator<T> filler, Expression<Func<T, TProp[]>> selector,
			int size, IValueGenerator<TProp> itemGenerator)
		{
			return filler.With(selector, new ArrayGenerator<TProp>(itemGenerator) { Size = size });
		}

		public static ObjectGenerator<T> WithArray<T, TProp>(this ObjectGenerator<T> filler,
			Expression<Func<T, IEnumerable<TProp>>> selector, int size, IEnumerable<TProp> range)
		{
			return filler.WithArray(selector, size, new RangeGenerator<TProp>(range));
		}

		public static ObjectGenerator<T> WithArray<T, TProp>(this ObjectGenerator<T> filler,
			Expression<Func<T, IEnumerable<TProp>>> selector, int size, IValueGenerator<TProp> itemGenerator)
		{
			return filler.With(selector, new ArrayGenerator<TProp>(itemGenerator) { Size = size });
		}

		public static ObjectGenerator<T> WithRange<T, TProp>(this ObjectGenerator<T> filler, Expression<Func<T, TProp>> selector,
			IEnumerable<TProp> range) where T : new()
		{
			return filler.With(selector, new RangeGenerator<TProp>(range));
		}

		public static ObjectGenerator<T> WithShortLorem<T>(this ObjectGenerator<T> filler, Expression<Func<T, string>> selector)
		{
			return filler.With(selector, new LoremGenerator
			{
				MinWords = 5,
				MaxWords = 10
			});
		}

		public static ObjectGenerator<T> WithLongLorem<T>(this ObjectGenerator<T> filler, Expression<Func<T, string>> selector)
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


		public static ObjectGenerator<T> WithHtmlLongLorem<T>(this ObjectGenerator<T> filler, Expression<Func<T, string>> selector)
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

		public static ObjectGenerator<T> WithPastDate<T>(this ObjectGenerator<T> filler, Expression<Func<T, DateTime?>> selector)
		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-12),
				To = DateTime.Now.AddMonths(-1)
			});
		}

		public static ObjectGenerator<T> WithPastDate<T>(this ObjectGenerator<T> filler, Expression<Func<T, DateTime>> selector)
		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-12),
				To = DateTime.Now.AddMonths(-1)
			});
		}

		public static ObjectGenerator<T> WithFutureDate<T>(this ObjectGenerator<T> filler,
			Expression<Func<T, DateTime?>> selector)
		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(+1),
				To = DateTime.Now.AddMonths(+12)
			});
		}

		public static ObjectGenerator<T> WithFutureDate<T>(this ObjectGenerator<T> filler,
			Expression<Func<T, DateTime>> selector)
		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(+1),
				To = DateTime.Now.AddMonths(+12)
			});
		}

		public static ObjectGenerator<T> WithActualDate<T>(this ObjectGenerator<T> filler, Expression<Func<T, DateTime?>> selector)
		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-1),
				To = DateTime.Now.AddDays(+1)
			});
		}

		public static ObjectGenerator<T> WithActualDate<T>(this ObjectGenerator<T> filler, Expression<Func<T, DateTime>> selector)
		{
			return filler.With(selector, new DateTimeRangeGenerator
			{
				From = DateTime.Now.AddMonths(-1),
				To = DateTime.Now.AddDays(+1)
			});
		}
	}
}