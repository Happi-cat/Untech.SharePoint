namespace Untech.SharePoint.Common.Configuration
{
	public abstract class BaseConfiguration
	{
		protected BaseConfiguration()
		{
			Mappings = new MappingsConfiguration();
			FieldConverters = new FieldConvertersConfiguration();
		}
		
		public MappingsConfiguration Mappings { get; private set; }

		public FieldConvertersConfiguration FieldConverters { get; private set; }


	}
}