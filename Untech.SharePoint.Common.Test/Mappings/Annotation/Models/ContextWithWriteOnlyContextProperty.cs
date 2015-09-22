using System;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class ContextWithWriteOnlyContextProperty : AnnotatedContext
	{
		[SpList(Title = "WriteOnlyProperty")]
		public ISpList<AnnotatedEntity> WriteOnlyProperty
		{
			set { throw new NotImplementedException(); }
		}
	}
}