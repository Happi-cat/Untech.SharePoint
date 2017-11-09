using Untech.SharePoint.CodeAnnotations;

namespace Untech.SharePoint.Data.Translators
{
	[PublicAPI]
	internal interface IProcessor<in TIn, out TOut>
	{
		TOut Process(TIn input);
	}
}