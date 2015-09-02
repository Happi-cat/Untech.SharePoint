namespace Untech.SharePoint.Common.Converters
{
	public interface IFieldConverter
	{
		object FromSpValue(object value);

		object ToSpValue(object value);

		string ToCamlValue(object value);
	}
}