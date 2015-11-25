using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Data.Translators
{
	[PublicAPI]
	internal interface IProcessor<in TIn, out TOut>
	{
		TOut Process(TIn input);
	}
}