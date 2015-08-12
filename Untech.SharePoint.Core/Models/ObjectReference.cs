using System;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Models
{
	[Serializable]
	public class ObjectReference
	{
		[JsonProperty]
		public int Id { get; set; }

		[JsonProperty]
		public string Value { get; set; }

		[JsonProperty]
		public Guid ListId { get; set; }
	}
}
