using Untech.SharePoint.Common.MetaModels;
namespace Untech.SharePoint.Common.Converters
{
	public interface IFieldConverter
	{
		void Initialize(MetaField field);

		object FromSpValue(object value);

		object ToSpValue(object value);

		string ToCamlValue(object value);
	}
}