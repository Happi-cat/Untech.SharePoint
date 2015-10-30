using System;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class CtxWithWriteOnlyContextProperty : Ctx
	{
		[SpList(Title = "WriteOnlyProperty")]
		public ISpList<Entity> WriteOnlyProperty
		{
			set { throw new NotImplementedException(); }
		}
	}
}