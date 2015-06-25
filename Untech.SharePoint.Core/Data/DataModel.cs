using System;
using System.Collections.Generic;
using Untech.SharePoint.Core.Reflection;

namespace Untech.SharePoint.Core.Data
{
	internal class DataModel
	{
		private readonly PropertyAccessor _propertyAccessor = new PropertyAccessor();
		private readonly DataModelPropertyInfos _propertyInfos = new DataModelPropertyInfos();

		internal IEnumerable<DataModelPropertyInfo> PropertyInfos
		{
			get { return _propertyInfos; }
		}

		internal PropertyAccessor PropertyAccessor
		{
			get { return _propertyAccessor; }
		}

		internal Type ModelType { get; private set; }

		internal DataMapper Mapper
		{
			get { return new DataMapper(this); }
		}

		internal void Initialize(Type modelType)
		{
			ModelType = modelType;

			_propertyAccessor.Initialize(modelType);
			_propertyInfos.Initialize(modelType);
		}
	}
}