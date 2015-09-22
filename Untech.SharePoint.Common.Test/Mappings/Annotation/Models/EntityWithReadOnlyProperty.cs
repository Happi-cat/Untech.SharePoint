using System;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class EntityWithReadOnlyProperty : AnnotatedEntity
	{
		[SpField]
		public string ReadonlyProperty
		{
			get { throw new NotImplementedException(); }
		}
	}
}