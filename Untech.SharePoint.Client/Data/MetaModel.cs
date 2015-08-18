using System;
using Untech.SharePoint.Client.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class MetaModel
	{
		public MetaModel()
		{
			PropertyAccessor = new PropertyAccessor();
			MetaProperties = new MetaProperties();
		}

		internal PropertyAccessor PropertyAccessor { get; private set; }

		internal Type ModelType { get; private set; }

		internal MetaProperties MetaProperties { get; private set; }

		internal DataMapper Mapper
		{
			get { return new DataMapper(this); }
		}

		internal void Initialize(Type modelType)
		{
			ModelType = modelType;

			PropertyAccessor.Initialize(modelType);
			MetaProperties.Initialize(modelType);
		}
	}
}