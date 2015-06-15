using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Core.Data
{
	public class DataMapperException : Exception
	{
		public DataMapperException(string propertyName, string internalName, Exception innerException) 
			: base("Unable to map field", innerException)
		{
			PropertyName = propertyName;
			InternalName = internalName;
		}

		public string PropertyName { get; private set; }

		public string InternalName { get; private set; }
	}
}
