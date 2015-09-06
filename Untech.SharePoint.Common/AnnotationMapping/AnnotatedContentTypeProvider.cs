using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedContentTypeProvider : IMetaContentTypeProvider
	{
		public AnnotatedContentTypeProvider(Type modelType)
		{
			ModelType = modelType;

			FieldProviders = GetFieldProviders(modelType)
				.ToList()
				.AsReadOnly();

			ContentTypeAttribute = modelType.GetCustomAttribute<SpContentTypeAttribute>();
		}

		public Type ModelType { get; private set; }

		public IReadOnlyCollection<IMetaFieldProvider> FieldProviders { get; private set; }

		public SpContentTypeAttribute ContentTypeAttribute { get; private set; }

		public MetaContentType GetMetaContentType(MetaList parent)
		{
			var metaContentType = new MetaContentType(parent, ModelType, FieldProviders);

			if (ContentTypeAttribute != null && !string.IsNullOrEmpty(ContentTypeAttribute.ContentTypeId))
			{
				metaContentType.ContentTypeId = ContentTypeAttribute.ContentTypeId;
			}

			return metaContentType;
		}

		private static IEnumerable<IMetaFieldProvider> GetFieldProviders(Type modelType)
		{
			var attributeType = typeof(SpFieldAttribute);

			var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Where(n => n.CanRead || n.CanWrite)
				.Where(n => !n.GetIndexParameters().Any())
				.Select(CreateFieldProvider)
				.ToList();

			var fields = modelType.GetFields(BindingFlags.Instance | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Select(CreateFieldProvider)
				.ToList();

			return properties.Concat(fields);
		}

		private static IMetaFieldProvider CreateFieldProvider(MemberInfo member)
		{
			return new AnnotatedFieldProvider(member);
		}
	}
}