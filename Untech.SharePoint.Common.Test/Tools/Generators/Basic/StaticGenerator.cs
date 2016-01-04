namespace Untech.SharePoint.Common.Test.Tools.Generators.Basic
{
	public class StaticGenerator<T> : IValueGenerator<T>
	{
		private readonly T _value;
		public StaticGenerator(T value)
		{
			_value = value;
		}

		public T Generate()
		{
			return _value;
		}
	}
}