using System;
using System.Text;

namespace Untech.SharePoint.TestTools.Generators.Basic
{
	public class LoremGenerator : BaseRandomGenerator, IValueGenerator<string>
	{
		private static readonly string[] s_words = {
			"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
			"adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
			"tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
		};

		public int MinWords { get; set; }

		public int MaxWords { get; set; }

		public int MinSentences { get; set; }

		public int MaxSentences { get; set; }

		public int ParagraphsNumber { get; set; }

		public bool Html { get; set; }

		public string Generate()
		{
			return LoremIpsum();
		}

		private string LoremIpsum()
		{
			var result = new StringBuilder();

			AppendForHtml(result, "<div>");
			var paragraphs = Math.Max(ParagraphsNumber, 1);
			for (var p = 0; p < paragraphs; p++)
			{
				LoremIpsumSentences(result);
			}
			AppendForHtml(result, "</div>");
			return result.ToString().Trim();
		}

		private void LoremIpsumSentences(StringBuilder sb)
		{
			AppendForHtml(sb, "<p>");
			var numSentences = Rand.Next(MaxSentences - MinSentences) + MinSentences + 1;
			for (var s = 0; s < numSentences; s++)
			{
				LoremIpsumSentence(sb);
			}
			AppendForHtml(sb, "</p>");
			sb.Append(Environment.NewLine);
		}

		private void LoremIpsumSentence(StringBuilder sb)
		{
			var numWords = Rand.Next(MaxWords - MinWords) + MinWords + 1;
			for (var w = 0; w < numWords; w++)
			{
				if (w > 0)
				{
					sb.Append(" ");
				}
				sb.Append(s_words[Rand.Next(s_words.Length)]);
			}
			sb.Append(". ");
		}

		private void AppendForHtml(StringBuilder sb, string value)
		{
			if (Html)
			{
				sb.Append(value);
			}
		}
	}
}