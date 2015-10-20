using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.MetaModels
{
	public interface IMetaModel
	{
		void Accept(IMetaModelVisitor visitor);

		T GetAdditionalProperty<T>(string key);

		void SetAdditionalProperty<T>(string key, T value);
	}
}
