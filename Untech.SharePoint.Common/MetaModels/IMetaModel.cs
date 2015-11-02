using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.MetaModels
{
	/// <summary>
	/// Represents base meta model interface.
	/// </summary>
	public interface IMetaModel
	{
		/// <summary>
		/// Accepts <see cref="IMetaModelVisitor"/> instance.
		/// </summary>
		/// <param name="visitor">Visitor to accept.</param>
		void Accept(IMetaModelVisitor visitor);

		/// <summary>
		/// Gets additional property value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type of the additional property value.</typeparam>
		/// <param name="key">The key of the additional property.</param>
		/// <returns>Property value associated with the specified key.</returns>
		T GetAdditionalProperty<T>(string key);

		/// <summary>
		/// Sets additional property value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type of the additional property value.</typeparam>
		/// <param name="key">The key of the additional property.</param>
		/// <param name="value">Property value associated with the specified key.</param>
		void SetAdditionalProperty<T>(string key, T value);
	}
}
