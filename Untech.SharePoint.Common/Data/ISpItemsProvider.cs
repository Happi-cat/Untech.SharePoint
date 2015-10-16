using System.Collections.Generic;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	public interface ISpItemsProvider
	{
		MetaList List { get; set; }

		IEnumerable<T> GetItems<T>(string caml);

		bool Any(string caml);

		int Count(string caml);

		T SingleOrDefault<T>(string caml);

		T FirstOrDefault<T>(string caml);

		T ElementAtOrDefault<T>(string caml, int index);

		void Add<T>(T item);

		void Update<T>(T item);
	}
}