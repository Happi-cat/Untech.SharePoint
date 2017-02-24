using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	internal class AnnotatedContentTypeMapping : IMetaContentTypeProvider
	{
		private readonly Type _entityType;
		private readonly List<AnnotatedFieldPart> _fieldParts;
		private readonly SpContentTypeAttribute _contentTypeAttrbiute;

		public AnnotatedContentTypeMapping(Type entityType)
		{
			Guard.CheckNotNull(nameof(entityType), entityType);

			_entityType = entityType;
			_contentTypeAttrbiute = _entityType.GetCustomAttribute<SpContentTypeAttribute>() ?? new SpContentTypeAttribute();

			_fieldParts = CreateFieldParts().ToList();
		}

		public MetaContentType GetMetaContentType(MetaList parent)
		{
			return new MetaContentType(parent, _entityType, _fieldParts)
			{
				Id = _contentTypeAttrbiute.Id,
				Name = _contentTypeAttrbiute.Name
			};
		}

		#region [Private Methods]

		private IEnumerable<AnnotatedFieldPart> CreateFieldParts()
		{
			var props = _entityType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(AnnotatedFieldPart.IsAnnotated);

			foreach (var prop in props)
			{
				yield return AnnotatedFieldPart.Create(prop);
			}

			var fields = _entityType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(AnnotatedFieldPart.IsAnnotated);

			foreach (var field in fields)
			{
				yield return AnnotatedFieldPart.Create(field);
			}
		}

		#endregion
	}
}