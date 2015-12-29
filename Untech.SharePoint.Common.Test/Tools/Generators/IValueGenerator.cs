namespace Untech.SharePoint.Common.Test.Tools.Generators
{
	public interface IValueGenerator<out T>
	{
		T Generate();
	}
}