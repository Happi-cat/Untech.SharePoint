using System.Text;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Test.Tools.Generators
{
	public class UrlGenerator : BaseRandomGenerator, IValueGenerator<string>, IValueGenerator<UrlInfo>
	{
		private static readonly string[] Urls = {
			"http://example.org",
			"http://example.com"
		};

		private static readonly string[] Words = {
			"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
			"adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
			"tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
		};

		public string Generate()
		{
			return Urls[Rand.Next(Urls.Length)] + LoremIpsumPath();
		}

		private string LoremIpsumPath()
		{
			var sb = new StringBuilder();
			var numWords = Rand.Next(5);
			for (var w = 0; w < numWords; w++)
			{
				sb.Append("/");
				sb.Append(Words[Rand.Next(Words.Length)]);
			}
			return sb.ToString();
		}

		UrlInfo IValueGenerator<UrlInfo>.Generate()
		{
			var url = Generate();
			var description = url
				.Replace("http://", "[")
				.Replace(".", "]: ")
				.Replace("/", " ");

			return new UrlInfo
			{
				Url = url,
				Description = description
			};
		}
	}
}