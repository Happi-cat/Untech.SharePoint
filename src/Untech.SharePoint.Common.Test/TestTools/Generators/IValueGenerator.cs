namespace Untech.SharePoint.TestTools.Generators
{
	public interface IValueGenerator<out T>
	{
		T Generate();
	}
}