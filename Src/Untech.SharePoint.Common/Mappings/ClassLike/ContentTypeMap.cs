using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Mappings.ClassLike
{
	/// <summary>
	/// Represents provider of <see cref="MetaContentType"/> that allows to configure content type mapping in fluent way.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity to map.</typeparam>
	[PublicAPI]
	public class ContentTypeMap<TEntity> : IMetaContentTypeProvider
	{
		private readonly Dictionary<MemberInfo, FieldPart> _fieldParts;
		private string _contentTypeId;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTypeMap{TEntity}"/>.
		/// </summary>
		public ContentTypeMap()
		{
			_fieldParts = new Dictionary<MemberInfo, FieldPart>(MemberInfoComparer.Default);
		}

		/// <summary>
		/// Sets content type id of the current or parent content type.
		/// </summary>
		/// <param name="contentTypeId"></param>
		public void ContentTypeId([CanBeNull]string contentTypeId)
		{
			_contentTypeId = contentTypeId;
		}

		/// <summary>
		/// Initializes a new or returns existing instance of the <see cref="FieldPart"/> for field mapping configuration.
		/// </summary>
		/// <param name="propertyAccessor">Field or property accessor.</param>
		/// <returns>Instance of the <see cref="FieldPart"/> that allows to configure content type's field mapping.</returns>
		[NotNull]
		public FieldPart Field([NotNull]Expression<Func<TEntity, object>> propertyAccessor)
		{
			Guard.CheckNotNull(nameof(propertyAccessor), propertyAccessor);

			var member = GetMemberInfo(propertyAccessor);

			if (!_fieldParts.ContainsKey(member))
			{
				CheckMember(member);

				_fieldParts.Add(member, new FieldPart(member));
			}

			return _fieldParts[member];
		}

		MetaContentType IMetaContentTypeProvider.GetMetaContentType(MetaList parent)
		{
			return new MetaContentType(parent, typeof(TEntity), _fieldParts.Values.ToList())
			{
				Id = _contentTypeId
			};
		}

		private void CheckMember(MemberInfo member)
		{
			if (member.MemberType == MemberTypes.Field)
			{
				Rules.CheckContentTypeField((FieldInfo)member);
			}
			else if (member.MemberType == MemberTypes.Property)
			{
				Rules.CheckContentTypeField((PropertyInfo)member);
			}
			else
			{
				throw new ArgumentException($"Invalid member info {member}");
			}
		}

		private MemberInfo GetMemberInfo(LambdaExpression lambda)
		{
			var body = lambda.Body.StripQuotes();
			if (body.NodeType.In(new[] { ExpressionType.Convert, ExpressionType.ConvertChecked }))
			{
				body = ((UnaryExpression)body).Operand;
			}
			if (body.NodeType == ExpressionType.MemberAccess)
			{
				return ((MemberExpression)body).Member;
			}
			throw new ArgumentException($"{lambda} is not a valid field or property accessor.");
		}
	}
}