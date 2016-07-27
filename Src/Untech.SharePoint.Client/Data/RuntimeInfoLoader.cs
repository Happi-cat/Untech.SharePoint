using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Client.Data
{
	internal class RuntimeInfoLoader : BaseMetaModelVisitor
	{
		public RuntimeInfoLoader(ClientContext clientContext)
		{
			ClientContext = clientContext;
		}

		private ClientContext ClientContext { get; }

		public override void VisitContext(MetaContext context)
		{
			context.Url = ClientContext.Url;
			base.VisitContext(context);
		}

		public override void VisitList(MetaList list)
		{
			var spList = GetList(list);

			list.Title = spList.Title;
			list.IsExternal = spList.HasExternalDataSource;

			new ListInfoLoader(spList).VisitList(list);
		}

		private List GetList(MetaList list)
		{
			try
			{
				return ClientContext.GetListByUrl(list.Url);
			}
			catch (Exception e)
			{
				throw new ListNotFoundException(list, e);
			}
		}

		private class ListInfoLoader : BaseMetaModelVisitor
		{
			public ListInfoLoader(List spList)
			{
				Common.Utils.Guard.CheckNotNull(nameof(spList), spList);

				SpList = spList;
			}

			private List SpList { get; }

			public override void VisitContentType(MetaContentType contentType)
			{
				var spContentType = SpList.ContentTypes
					.Where(n => n.StringId.StartsWith(contentType.Id ?? "0x01"))
					.OrderBy(n => n.StringId.Length)
					.FirstOrDefault();

				if (spContentType == null)
				{
					throw new ContentTypeNotFoundException(contentType);
				}

				contentType.Id = spContentType.Id.ToString();
				contentType.Name = spContentType.Name;

				base.VisitContentType(contentType);
			}

			public override void VisitField(MetaField field)
			{
				var spField = GetField(field);

				field.Id = spField.Id;
				field.Title = spField.Title;
				field.ReadOnly = spField.ReadOnlyField;
				field.Required = spField.Required;

				field.IsCalculated = spField.FieldTypeKind == FieldType.Computed || spField.FieldTypeKind == FieldType.Calculated;
				if (string.IsNullOrEmpty(field.TypeAsString))
				{
					field.TypeAsString = spField.TypeAsString;
				}

				if (spField.FieldTypeKind == FieldType.Lookup)
				{
					var spLookupField = SpList.Context.CastTo<FieldLookup>(spField);

					field.AllowMultipleValues = spLookupField.AllowMultipleValues;
					field.LookupList = spLookupField.LookupList;
					field.LookupField = spLookupField.LookupField;
				}
				if (spField.FieldTypeKind == FieldType.User)
				{
					var spUserField = SpList.Context.CastTo<FieldUser>(spField);

					field.AllowMultipleValues = spUserField.AllowMultipleValues;
				}
				if (spField.FieldTypeKind == FieldType.MultiChoice)
				{
					field.AllowMultipleValues = true;
				}
				if (spField.FieldTypeKind == FieldType.Calculated)
				{
					var spCalculatedField = SpList.Context.CastTo<FieldCalculated>(spField);
					field.OutputType = spCalculatedField.OutputType.ToString();
				}
			}

			private Field GetField(MetaField field)
			{
				try
				{
					var spField = SpList.Fields.GetByInternalNameOrTitle(field.InternalName);

					SpList.Context.Load(spField);
					SpList.Context.ExecuteQuery();

					return spField;
				}
				catch (Exception e)
				{
					throw new FieldNotFoundException(field, e);
				}
			}
		}
	}
}