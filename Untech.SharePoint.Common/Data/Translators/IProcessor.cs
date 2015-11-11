namespace Untech.SharePoint.Common.Data.Translators
{
	internal interface IProcessor<in TIn, out TOut>
	{
		TOut Process(TIn input);
	}
}