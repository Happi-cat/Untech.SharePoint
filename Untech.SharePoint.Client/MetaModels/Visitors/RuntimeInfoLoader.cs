using System.Collections.Generic;
using Microsoft.SharePoint.Client;
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
			var spList = ClientContext.Web.Lists.GetByTitle(list.Title);

			list.IsExternal = spList.HasExternalDataSource;

			new ListInfoLoader(spList).VisitList(list);
		}

		internal class ListInfoLoader : BaseMetaModelVisitor
		{
			public ListInfoLoader(List spList, IList<Field> spFields)
			{
				Common.Utils.Guard.CheckNotNull("spList", spList);

				SpList = spList;
				SpFields = spFields;
			}

			public List SpList { get; private set; }

			public IList<Field> SpFields { get; private set; }

			public override void VisitContentType(MetaContentType contentType)
			{
				var bestMatch = SpList.ContentTypes.BestMatch(new SPContentTypeId(contentType.Id));
				var spContentType = SpList.ContentTypes[bestMatch];

				contentType.Id = spContentType.Id.ToString();
				contentType.Name = spContentType.Name;

				base.VisitContentType(contentType);
			}

			public override void VisitField(MetaField field)
			{
				var spField = SpList.Fields.GetFieldByInternalName(field.InternalName);

				field.Id = spField.Id;
				field.Title = spField.Title;
				field.ReadOnly = spField.ReadOnlyField;
				field.Required = spField.Required;

				field.IsCalculated = spField.Type == SPFieldType.Computed || spField.Type == SPFieldType.Calculated;
				field.TypeAsString = spField.TypeAsString;

				if (spField.Type == SPFieldType.Lookup)
				{
					var spLookupField = (SPFieldLookup)spField;

					field.AllowMultipleValues = spLookupField.AllowMultipleValues;
					field.LookupList = spLookupField.LookupList;
					field.LookupField = spLookupField.LookupField;
				}
				if (spField.Type == SPFieldType.MultiChoice)
				{
					field.AllowMultipleValues = true;
				}
				if (spField.Type == SPFieldType.Calculated)
				{
					var spCalculatedField = (SPFieldCalculated) spField;
					field.TypeAsString = spCalculatedField.OutputType.ToString();
				}
			}
		}
	}
}