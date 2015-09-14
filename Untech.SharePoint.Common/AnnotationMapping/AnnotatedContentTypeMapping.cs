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
		private readonly Type _entityType;
		private readonly List<AnnotatedFieldPart> _fieldParts;
		private readonly SpContentTypeAttribute _contentTypeAttrbiute;

		protected AnnotatedContentTypeMapping(Type entityType)
		{
			Guard.CheckNotNull("entityType", entityType);

			_entityType = entityType;
			_fieldParts = new List<AnnotatedFieldPart>();
			_contentTypeAttrbiute = _entityType.GetCustomAttribute<SpContentTypeAttribute>();
		}

		public string ContentTypeId
		{
			get { return _contentTypeAttrbiute != null ? _contentTypeAttrbiute.Id : string.Empty; }
		}

		public string ContentTypeName
		{
			get { return _contentTypeAttrbiute.Name; }
		}

		public static AnnotatedContentTypeMapping Create(Type entityType)
		{
			var mapping = new AnnotatedContentTypeMapping(entityType);

			mapping._fieldParts.AddRange(entityType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(AnnotationConventions.HasFieldAnnotation)
				.Select(AnnotatedFieldPart.Create));

			mapping._fieldParts.AddRange(entityType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(AnnotationConventions.HasFieldAnnotation)
				.Select(AnnotatedFieldPart.Create));

			return mapping;
		}

		public MetaContentType GetMetaContentType(MetaList parent)
		{
			return new MetaContentType(parent, _entityType, _fieldParts)
			{
				Id = ContentTypeId,
				Name = ContentTypeName
			};
		}
	}
}