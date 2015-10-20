using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.MetaModels
{
	public abstract class BaseMetaModel : IMetaModel
	{
		protected BaseMetaModel()
		{
			AdditionalProperties  = new Container<string, object>();
		}

		protected Container<string, object> AdditionalProperties { get; private set; }

		public abstract void Accept(IMetaModelVisitor visitor);

		public virtual T GetAdditionalProperty<T>(string key)
		{
			return (T) AdditionalProperties.Resolve(key);
		}

		public virtual void SetAdditionalProperty<T>(string key, T value)
		{
			AdditionalProperties.Register(key, value);
		}
	}
}