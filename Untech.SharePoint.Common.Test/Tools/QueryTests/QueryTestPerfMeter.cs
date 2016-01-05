using System;
using System.IO;
using System.Linq;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class QueryTestPerfMeter<T> : QueryTest<T>
	{
		public const int Attempts = 1000;

		public QueryTestPerfMeter(string fileName, string category, QueryTest<T> innerTest)
		{
			Inner = innerTest;
			Category = category;
			FilePath = fileName;

			if (!File.Exists(FilePath))
			{
				CreateFile();
			} 
		}

		public string FilePath { get; private set; }

		public string Category { get; set; }

		public QueryTest<T> Inner { get; private set; }

		public override void Test(ISpList<T> list, IQueryable<T> alternateList)
		{
			var attempts = Attempts;
			while (attempts-- > 0)
			{
				Inner.Test(list, alternateList);
			}
			LogResult();
		}

		public override TimeSpan GetElapsedTime()
		{
			return Inner.GetElapsedTime();
		}

		public override int GetItemsCounter()
		{
			return Inner.GetItemsCounter();
		}

		private void LogResult()
		{
			using (var file = File.AppendText(FilePath))
			{
				var items = GetItemsCounter();
				var elapsedTime = GetElapsedTime();
				file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", Category, Inner, Attempts, GetItemsCounter(), elapsedTime.Ticks,
					elapsedTime, new TimeSpan(elapsedTime.Ticks/Attempts), items/Attempts);
			}
		}

		private void CreateFile()
		{
			using (var file = File.CreateText(FilePath))
			{
				file.WriteLine("Category;Query;Attempts;Items;Ticks;Timespan;TimespanPerAttempt;ItemsPerAttempt");
			}
		}
	}
}