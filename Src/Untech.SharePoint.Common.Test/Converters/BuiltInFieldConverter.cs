namespace Untech.SharePoint.Common.Converters
{
	[SpFieldConverter("BUILT_IN_TEST_CONVERTER")]
	public class BuiltInTestFieldConverter : IFieldConverter
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