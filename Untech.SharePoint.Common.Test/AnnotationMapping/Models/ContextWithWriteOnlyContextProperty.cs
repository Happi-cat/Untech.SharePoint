using System;
using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
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