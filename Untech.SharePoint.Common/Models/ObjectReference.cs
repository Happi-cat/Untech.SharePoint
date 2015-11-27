using System;
using Newtonsoft.Json;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents lookup field value
	/// </summary>
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	[Serializable]
	public class ObjectReference
	{
		/// <summary>
		/// Gets or sets lookup id.
		/// </summary>
		[JsonProperty]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets lookup value.
		/// </summary>
		[JsonProperty]
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets list id.
		/// </summary>
		[JsonProperty]
		public Guid ListId { get; set; }
	}
}
