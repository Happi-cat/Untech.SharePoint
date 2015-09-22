using System;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class EntityWithWriteOnlyProperty : AnnotatedEntity
	{
		[SpField]
		public string WriteonlyProperty
		{
			get { throw new NotImplementedException(); }
		}
	}
}