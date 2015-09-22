using System;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class ContextWithWriteOnlyContextProperty : TestContext
	{
		[SpList(Title = "WriteOnlyProperty")]
		public ISpList<TestEntity> WriteOnlyProperty
		{
			set { throw new NotImplementedException(); }
		}
	}
}