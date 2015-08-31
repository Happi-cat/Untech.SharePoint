using System;

namespace Untech.SharePoint.Client.Data
{
	public interface IEntity
	{
		int ID { get; set; }

		string Title { get; set; }

		DateTime Created { get; set; }

		DateTime? Modified { get; set; }
	}
}