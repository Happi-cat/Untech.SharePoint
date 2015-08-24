using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Client.Data;

namespace Untech.SharePoint.Client.Test.Data
{
	class ExampleModel 
	{

	}

	class ExampleContext
	{
		const string str = "12";
		public IQueryable<ExampleModel> List1 {
			get { return GetList<ExampleModel>("list1"); }
		}

		public IQueryable<ExampleModel> List2
		{
			get { return GetList<ExampleModel>(Guid.Empty); }
		}

		[SpList(str)]
		public IQueryable<ExampleModel> List3
		{
			get { return GetList<ExampleModel>(n => n.List3); }
		}
		

		private IQueryable<T> GetList<T>(string listTitle)
		{
			throw new NotImplementedException();
		}

		private IQueryable<T> GetList<T>(Guid guid)
		{
			throw new NotImplementedException();
		}

		private IQueryable<T> GetList<T>(Expression<Func<ExampleContext, IQueryable<T>>> prop)
		{
			throw new NotImplementedException();
		}
	}
}
