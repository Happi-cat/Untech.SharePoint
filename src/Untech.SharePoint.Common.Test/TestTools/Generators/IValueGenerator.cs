namespace Untech.SharePoint.Common.TestTools.Generators
{
	public interface IValueGenerator<out T>
	{
		T Generate();
	}
}