using System;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Mappings
{
	/// <summary>
	/// Represents class that can create <see cref="MetaContext"/> and resolve list title for the specified member of this context.
	/// </summary>
	/// <typeparam name="TContext">Type of the data context that is associated with this instance of the <see cref="MappingSource{TContext}"/></typeparam>
	[PublicAPI]
	public abstract class MappingSource<TContext> : IMappingSource 
		where TContext : ISpContext
	{
		/// <summary>
		/// Gets <see cref="Type"/> of the associated Data Context class.
		/// </summary>
		public Type ContextType => typeof(TContext);

		/// <summary>
		/// Returns instance of <see cref="MetaContext"/>.
		/// </summary>
		/// <returns>New instance of <see cref="MetaContext"/></returns>
		public abstract MetaContext GetMetaContext();

		/// <summary>
		/// Gets list title that associated with <paramref name="member"/>.
		/// </summary>
		/// <param name="member">Member to resolve.</param>
		/// <returns>List title that associated with <paramref name="member"/>.</returns>
		public abstract string GetListTitleFromContextMember(MemberInfo member);
	}
}