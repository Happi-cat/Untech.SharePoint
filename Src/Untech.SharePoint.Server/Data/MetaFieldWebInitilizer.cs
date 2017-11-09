using Microsoft.SharePoint;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.MetaModels.Visitors;

namespace Untech.SharePoint.Server.Data
{
	internal class MetaFieldWebInitilizer : BaseMetaModelVisitor
	{
		public MetaFieldWebInitilizer(SPWeb web)
		{
			SpWeb = web;
		}

		private SPWeb SpWeb { get; }

		public override void VisitField(MetaField field)
		{
			field.SetSpWeb(SpWeb);
		}
	}
}