namespace Untech.SharePoint.Common.Test.Tools.Generators
{
	public interface IValueFiller<T> : IValueGenerator<T>
	{
		void Fill(T item);
	}
}