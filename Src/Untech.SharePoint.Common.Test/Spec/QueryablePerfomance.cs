using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Spec.Models;
using Untech.SharePoint.TestTools.QueryTests;

namespace Untech.SharePoint.Spec
{
	public class QueryablePerfomance : ITestQueryProvider<NewsModel>
	{
		[QueryCaml(@"<View>
			  <Query>
				<Where>
				  <Eq>
					<FieldRef Name='ContentTypeId' />
					<Value>{0}</Value>
				  </Eq>
				</Where>
			  </Query>
			  <ViewFields>
				<FieldRef Name='Body' />
				<FieldRef Name='Description' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
				<FieldRef Name='Created' />
				<FieldRef Name='Author' />
				<FieldRef Name='Modified' />
				<FieldRef Name='Editor' />
				<FieldRef Name='ContentTypeId' />
			  </ViewFields>
			</View>", "Body,Description,HeadingImage,HeadingImage,ID,Title,Created,Author,Modified,Editor,ContentTypeId")]
		public IEnumerable<NewsModel> FetchAll(IQueryable<NewsModel> source)
		{
			return source;
		}

		[QueryCaml(@"<View>
			  <Query>
				<Where>
				  <Eq>
					<FieldRef Name='ContentTypeId' />
					<Value>{0}</Value>
				  </Eq>
				</Where>
			  </Query>
			  <ViewFields>
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
			  </ViewFields>
			</View>", "ID,Title")]
		public IEnumerable<Tuple<int, string>> SelectIdTitle(IQueryable<NewsModel> source)
		{
			return source.Select(n => new Tuple<int, string>(n.Id, n.Title));
		}

		[QueryCaml(@"<View>
			  <Query>
				<Where>
				  <And>
					<Or>
					  <Contains>
						<FieldRef Name='Title' />
						<Value Type='Text'>lorem</Value>
					  </Contains>
					  <Contains>
						<FieldRef Name='Description' />
						<Value Type='Note'>DESCRIPTION</Value>
					  </Contains>
					</Or>
					<Eq>
					  <FieldRef Name='ContentTypeId' />
					  <Value>{0}</Value>
					</Eq>
				  </And>
				</Where>
			  </Query>
			  <ViewFields>
				<FieldRef Name='Body' />
				<FieldRef Name='Description' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
				<FieldRef Name='Created' />
				<FieldRef Name='Author' />
				<FieldRef Name='Modified' />
				<FieldRef Name='Editor' />
				<FieldRef Name='ContentTypeId' />
			  </ViewFields>
			</View>", "Body,Description,HeadingImage,HeadingImage,ID,Title,Created,Author,Modified,Editor,ContentTypeId")]
		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"));
		}

		[QueryCaml(@"<View>
			  <RowLimit>10</RowLimit>
			  <Query>
				<Where>
				  <Eq>
					<FieldRef Name='ContentTypeId' />
					<Value>{0}</Value>
				  </Eq>
				</Where>
			  </Query>
			  <ViewFields>
				<FieldRef Name='Body' />
				<FieldRef Name='Description' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
				<FieldRef Name='Created' />
				<FieldRef Name='Author' />
				<FieldRef Name='Modified' />
				<FieldRef Name='Editor' />
				<FieldRef Name='ContentTypeId' />
			  </ViewFields>
			</View>", "Body,Description,HeadingImage,HeadingImage,ID,Title,Created,Author,Modified,Editor,ContentTypeId")]
		public IEnumerable<NewsModel> Take10(IQueryable<NewsModel> source)
		{
			return source
				.Take(10);
		}

		[QueryCaml(@"<View>
			  <RowLimit>10</RowLimit>
			  <Query>
				<Where>
				  <And>
					<Or>
					  <Contains>
						<FieldRef Name='Title' />
						<Value Type='Text'>lorem</Value>
					  </Contains>
					  <Contains>
						<FieldRef Name='Description' />
						<Value Type='Note'>DESCRIPTION</Value>
					  </Contains>
					</Or>
					<Eq>
					  <FieldRef Name='ContentTypeId' />
					  <Value>{0}</Value>
					</Eq>
				  </And>
				</Where>
			  </Query>
			  <ViewFields>
				<FieldRef Name='Body' />
				<FieldRef Name='Description' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
				<FieldRef Name='Created' />
				<FieldRef Name='Author' />
				<FieldRef Name='Modified' />
				<FieldRef Name='Editor' />
				<FieldRef Name='ContentTypeId' />
			  </ViewFields>
			</View>", "Body,Description,HeadingImage,HeadingImage,ID,Title,Created,Author,Modified,Editor,ContentTypeId")]
		public IEnumerable<NewsModel> WhereTake10Query(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"))
				.Take(10);
		}

		[QueryCaml(@"<View>
			  <RowLimit>1</RowLimit>
			  <Query>
				<Where>
				  <And>
					<Or>
					  <Contains>
						<FieldRef Name='Title' />
						<Value Type='Text'>lorem</Value>
					  </Contains>
					  <Contains>
						<FieldRef Name='Description' />
						<Value Type='Note'>DESCRIPTION</Value>
					  </Contains>
					</Or>
					<Eq>
					  <FieldRef Name='ContentTypeId' />
					  <Value>{0}</Value>
					</Eq>
				  </And>
				</Where>
			  </Query>
			  <ViewFields>
				<FieldRef Name='Body' />
				<FieldRef Name='Description' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
				<FieldRef Name='Created' />
				<FieldRef Name='Author' />
				<FieldRef Name='Modified' />
				<FieldRef Name='Editor' />
				<FieldRef Name='ContentTypeId' />
			  </ViewFields>
			</View>", "Body,Description,HeadingImage,HeadingImage,ID,Title,Created,Author,Modified,Editor,ContentTypeId")]
		public NewsModel WhereFirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.First(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"));
		}

		[QueryCaml(@"<View>
			  <RowLimit>1</RowLimit>
			  <Query>
				<Where>
				  <And>
					<Or>
					  <Contains>
						<FieldRef Name='Title' />
						<Value Type='Text'>lorem</Value>
					  </Contains>
					  <Contains>
						<FieldRef Name='Description' />
						<Value Type='Note'>DESCRIPTION</Value>
					  </Contains>
					</Or>
					<Eq>
					  <FieldRef Name='ContentTypeId' />
					  <Value>{0}</Value>
					</Eq>
				  </And>
				</Where>
				<OrderBy>
				  <FieldRef Ascending='FALSE' Name='ID' />
				</OrderBy>
			  </Query>
			  <ViewFields>
				<FieldRef Name='Body' />
				<FieldRef Name='Description' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='HeadingImage' />
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
				<FieldRef Name='Created' />
				<FieldRef Name='Author' />
				<FieldRef Name='Modified' />
				<FieldRef Name='Editor' />
				<FieldRef Name='ContentTypeId' />
			  </ViewFields>
			</View>", "Body,Description,HeadingImage,HeadingImage,ID,Title,Created,Author,Modified,Editor,ContentTypeId")]
		public NewsModel WhereLastQuery(IQueryable<NewsModel> source)
		{
			return source
				.Last(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"));
		}

		public IEnumerable<Func<IQueryable<NewsModel>, object>> GetQueries()
		{
			return new Func<IQueryable<NewsModel>, object>[]
			{
				FetchAll,
				SelectIdTitle,
				WhereQuery,
				Take10,
				WhereTake10Query,
				WhereFirstQuery,
				WhereLastQuery
			};
		}
	}
}