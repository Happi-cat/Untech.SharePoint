using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Client.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
	public sealed class SpListAttribute : Attribute
	{
		public SpListAttribute(string listTile)
		{
			ListTitle = listTile;
		}

		public string ListTitle { get; private set; }
	}
}
