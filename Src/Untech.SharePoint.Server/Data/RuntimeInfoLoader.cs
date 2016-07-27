using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Extensions;

namespace Untech.SharePoint.Server.Data
{
	internal class RuntimeInfoLoader : BaseMetaModelVisitor
	{
		public RuntimeInfoLoader(SPWeb spWeb)
		{
			SpWeb = spWeb;
		}

		private SPWeb SpWeb { get; }

		public override void VisitContext(MetaContext context)
		{
			context.Url = SpWeb.Url;
			base.VisitContext(context);
		}

		public override void VisitList(MetaList list)
		{
			var spList = GetList(list);

			list.Title = spList.Title;
			list.IsExternal = spList.HasExternalDataSource;

			new ListInfoLoader(spList).VisitList(list);
		}

		private SPList GetList(MetaList list)
		{
			try
			{
				return SpWeb.GetListByUrl(list.Url);
			}
			catch (Exception e)
			{
				throw new ListNotFoundException(list, e);
			}
		}

		private class ListInfoLoader : BaseMetaModelVisitor
		{
			public ListInfoLoader(SPList spList)
			{
				Guard.CheckNotNull(nameof(spList), spList);

				SpList = spList;
			}

			private SPList SpList { get; }

			public override void VisitContentType(MetaContentType contentType)
			{
				var spContentType = SpList.ContentTypes
					.Cast<SPContentType>()
					.Select(n => new { ContentType = n, StringId = n.Id.ToString() })
					.Where(n => n.StringId.StartsWith(contentType.Id ?? "0x01"))
					.OrderBy(n => n.StringId.Length)
					.Select(n => n.ContentType)
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

				field.IsCalculated = spField.Type == SPFieldType.Computed || spField.Type == SPFieldType.Calculated;
				if (string.IsNullOrEmpty(field.TypeAsString))
				{
					field.TypeAsString = spField.TypeAsString;
				}

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
				if (spField.Type == SPFieldType.User)
				{
					var spuserField = (SPFieldUser)spField;

					field.AllowMultipleValues = spuserField.AllowMultipleValues;
				}
				if (spField.Type == SPFieldType.Calculated)
				{
					var spCalculatedField = (SPFieldCalculated) spField;
					field.OutputType = spCalculatedField.OutputType.ToString();
				}
			}

			private SPField GetField(MetaField field)
			{
				try
				{
					return SpList.Fields.GetFieldByInternalName(field.InternalName);
				}
				catch (Exception e)
				{
					throw new FieldNotFoundException(field, e);
				}
			}
		}
	}
}