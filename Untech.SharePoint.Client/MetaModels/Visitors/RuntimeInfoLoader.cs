using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Client.MetaModels.Visitors
{
	internal class RuntimeInfoLoader : BaseMetaModelVisitor
	{
		public RuntimeInfoLoader(ClientContext clientContext)
		{
			ClientContext = clientContext;
		}

		public ClientContext ClientContext { get; private set; }

		public override void VisitList(MetaList list)
		{
			var spList = ClientContext.GetList(list.Title);

			list.IsExternal = spList.HasExternalDataSource;

			new ListInfoLoader(spList).VisitList(list);
		}

		internal class ListInfoLoader : BaseMetaModelVisitor
		{
			public ListInfoLoader(List spList)
			{
				Common.Utils.Guard.CheckNotNull("spList", spList);

				SpList = spList;
			}

			public List SpList { get; private set; }

			public override void VisitContentType(MetaContentType contentType)
			{
				var spContentType = SpList.ContentTypes.OrderBy(n => n.StringId).First(n => n.StringId.StartsWith(contentType.Id));

				contentType.Id = spContentType.Id.ToString();
				contentType.Name = spContentType.Name;

				base.VisitContentType(contentType);
			}

			public override void VisitField(MetaField field)
			{
				var spField = SpList.Fields.GetByInternalNameOrTitle(field.InternalName);

				SpList.Context.Load(spField);
				SpList.Context.ExecuteQuery();

				field.Id = spField.Id;
				field.Title = spField.Title;
				field.ReadOnly = spField.ReadOnlyField;
				field.Required = spField.Required;

				field.IsCalculated = spField.FieldTypeKind == FieldType.Computed || spField.FieldTypeKind == FieldType.Calculated;
				field.TypeAsString = spField.TypeAsString;

				if (spField.FieldTypeKind == FieldType.Lookup)
				{
					var spLookupField = (FieldLookup)spField;

					field.AllowMultipleValues = spLookupField.AllowMultipleValues;
					field.LookupList = spLookupField.LookupList;
					field.LookupField = spLookupField.LookupField;
				}
				if (spField.FieldTypeKind == FieldType.MultiChoice)
				{
					field.AllowMultipleValues = true;
				}
				if (spField.FieldTypeKind == FieldType.Calculated)
				{
					var spCalculatedField = (FieldCalculated) spField;
					field.TypeAsString = spCalculatedField.OutputType.ToString();
				}
			}
		}
	}
}