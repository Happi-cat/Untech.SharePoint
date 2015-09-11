using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedContentTypeMapping : IMetaContentTypeProvider
	{
		private List<AnnotatedFieldPart> _fieldParts;

		public AnnotatedContentTypeMapping(Type entityType)
		{
			Guard.CheckNotNull("entityType", entityType);

			EntityType = entityType;

			ContentTypeAttribute = entityType.GetCustomAttribute<SpContentTypeAttribute>();

			InitFieldParts();
		}

		public Type EntityType { get; private set; }

		public IReadOnlyCollection<AnnotatedFieldPart> FieldParts { get { return _fieldParts; } }

		public SpContentTypeAttribute ContentTypeAttribute { get; private set; }

		public string ContentTypeId
		{
			get
			{
				return ContentTypeAttribute != null ? ContentTypeAttribute.Id : string.Empty;
			}
		}

		public string ContentTypeName
		{
			get { return ContentTypeAttribute.Name; }
		}

		public MetaContentType GetMetaContentType(MetaList parent)
		{
			var metaContentType = new MetaContentType(parent, EntityType, FieldParts)
			{
				Id = ContentTypeId,
				Name = ContentTypeName
			};

			return metaContentType;
		}

		private void InitFieldParts()
		{
			_fieldParts = new List<AnnotatedFieldPart>();

			var fieldAttribute = typeof(SpFieldAttribute);

			EntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(n => n.IsDefined<SpFieldAttribute>())
				.Where(n => !n.IsDefined<SpFieldRemovedAttribute>())
				.Where(n => n.CanRead || n.CanWrite)
				.Where(n => n.GetIndexParameters().IsNullOrEmpty())
				.Each(RegisterFieldPart);

			EntityType.GetFields(BindingFlags.Instance | BindingFlags.Public)
				.Where(n => n.IsDefined<SpFieldAttribute>())
				.Where(n => !n.IsDefined<SpFieldRemovedAttribute>())
				.Where(n => !n.IsInitOnly && !n.IsLiteral)
				.Each(RegisterFieldPart);
		}

		private void RegisterFieldPart(MemberInfo member)
		{
			_fieldParts.Add(new AnnotatedFieldPart(member));
		}
	}
}