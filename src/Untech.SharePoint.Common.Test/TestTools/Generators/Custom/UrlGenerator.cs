using System.Text;
using Untech.SharePoint.Models;
using Untech.SharePoint.TestTools.Generators.Basic;

namespace Untech.SharePoint.TestTools.Generators.Custom
{
	public class UrlGenerator : BaseRandomGenerator, IValueGenerator<string>, IValueGenerator<UrlInfo>
	{
		private static readonly string[] s_urls = {
			"http://example.org",
			"http://example.com"
		};

		private static readonly string[] s_words = {
			"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
			"adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
			"tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
		};

		public string Generate()
		{
			return s_urls[Rand.Next(s_urls.Length)] + LoremIpsumPath();
		}

		private string LoremIpsumPath()
		{
			var sb = new StringBuilder();
			var numWords = Rand.Next(5);
			for (var w = 0; w < numWords; w++)
			{
				sb.Append("/");
				sb.Append(s_words[Rand.Next(s_words.Length)]);
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