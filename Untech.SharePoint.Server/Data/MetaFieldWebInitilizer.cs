using Microsoft.SharePoint;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Server.Data
{
	internal class MetaFieldWebInitilizer : BaseMetaModelVisitor
	{
		public MetaFieldWebInitilizer(SPWeb web)
		{
			SpWeb = web;
		}

		protected SPWeb SpWeb { get; private set; }

		public override void VisitField(MetaField field)
		{
			field.SetSpWeb(SpWeb);
		}
	}
}