using Microsoft.SharePoint;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.MetaModels.Visitors
{
	internal class RuntimeInfoLoader : BaseMetaModelVisitor
	{
		public RuntimeInfoLoader(SPWeb spWeb)
		{
			SpWeb = spWeb;
		}

		public SPWeb SpWeb { get; private set; }

		public override void VisitList(MetaList list)
		{
			new ListInfoLoader(SpWeb.Lists[list.Title])
				.VisitList(list);
		}

		internal class ListInfoLoader : BaseMetaModelVisitor
		{
			public ListInfoLoader(SPList spList)
			{
				Guard.CheckNotNull("spList", spList);

				SpList = spList;
			}

			public SPList SpList { get; private set; }

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