using Untech.SharePoint.Common.Converters;

namespace Untech.SharePoint.Common.Test.Configuration
{
	[SpFieldConverter("BUILT_IN_TEST_CONVERTER")]
	public class BuiltInFieldConverter : IFieldConverter
	{

		public void Initialize(MetaModels.MetaField field)
		{
		}

		public object FromSpValue(object value)
		{
			throw new System.NotImplementedException();
		}

		public object ToSpValue(object value)
		{
			throw new System.NotImplementedException();
		}

		public string ToCamlValue(object value)
		{
			throw new System.NotImplementedException();
		}
	}
}