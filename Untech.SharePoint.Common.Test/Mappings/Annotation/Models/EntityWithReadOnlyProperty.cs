using System;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class EntityWithReadOnlyProperty : Entity
	{
		[SpField]
		public string ReadonlyProperty
		{
			get { throw new NotImplementedException(); }
		}
	}
}