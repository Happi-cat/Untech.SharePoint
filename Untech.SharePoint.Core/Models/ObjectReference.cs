using System;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Models
{
	[Serializable]
	public class ObjectReference
	{
		public ObjectReference()
		{ }

		public ObjectReference(Guid listId, int id, string value)
		{
			ListId = listId;
			Id = id;
			Value = value;
		}

		[JsonProperty]
		public int Id { get; set; }

		[JsonProperty]
		public string Value { get; set; }

		[JsonProperty]
		public Guid ListId { get; set; }
	}
}
