using Untech.SharePoint.Models;

namespace Untech.SharePoint.Mappings.ClassLike
{
	/// <summary>
	/// Represents base <see cref="ContentTypeMap{TEntity}"/> for types derived from <see cref="Entity"/>.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity to map.</typeparam>
	public class EntityContentTypeMap<TEntity> : ContentTypeMap<TEntity>
		where TEntity : Entity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityContentTypeMap{TEntity}"/>.
		/// </summary>
		public EntityContentTypeMap()
		{
			ContentTypeId("0x01");

			Field(n => n.Id).InternalName("ID").TypeAsString("Counter");
			Field(n => n.ContentTypeId).TypeAsString("ContentTypeId");

			Field(n => n.Title).TypeAsString("Text");

			Field(n => n.Created).TypeAsString("DateTime");
			Field(n => n.Modified).TypeAsString("DateTime");

			Field(n => n.Author).TypeAsString("User");
			Field(n => n.Editor).TypeAsString("User");
		}
	}
}