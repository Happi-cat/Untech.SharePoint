using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class QueryablePerfomance : IQueryTestsProvider<NewsModel>
	{
		public IEnumerable<QueryTest<NewsModel>> GetQueryTests()
		{
			return new[]
			{
				QueryTest<NewsModel>.Perfomance(FetchAll, FetchAllCamlQuery()),
				QueryTest<NewsModel>.Perfomance(SelectIdTitle, SelectidTitleCaml()),
				QueryTest<NewsModel>.Perfomance(WhereQuery, WhereCamlQuery()),
				QueryTest<NewsModel>.Perfomance(Take10, Take10CamlQuery()),
				QueryTest<NewsModel>.Perfomance(WhereTake10Query, WhereTake10CamlQuery()),
				QueryTest<NewsModel>.Perfomance(WhereFirstQuery, WhereFirstQuery()),
				QueryTest<NewsModel>.Perfomance(WhereLastQuery, WhereLastCamlQuery())
			};
		}

		public IEnumerable<NewsModel> FetchAll(IQueryable<NewsModel> source)
		{
			return source;
		}

		public string FetchAllCamlQuery()
		{
			return @"<View>
			  <Query>
				<Where>
				  <Eq>
					<FieldRef Name='ContentTypeId' />
					<Value>0x0100517159DC631A6D4CBD397E03E431C0C6</Value>
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
			</View>";
		}

		public IEnumerable<Tuple<int, string>> SelectIdTitle(IQueryable<NewsModel> source)
		{
			return source.Select(n => new Tuple<int, string>(n.Id, n.Title));
		}

		public string SelectidTitleCaml()
		{
			return @"<View>
			  <Query>
				<Where>
				  <Eq>
					<FieldRef Name='ContentTypeId' />
					<Value>0x0100517159DC631A6D4CBD397E03E431C0C6</Value>
				  </Eq>
				</Where>
			  </Query>
			  <ViewFields>
				<FieldRef Name='ID' />
				<FieldRef Name='Title' />
			  </ViewFields>
			</View>";
		}

		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"));
		}

		public string WhereCamlQuery()
		{
			return @"<View>
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
					  <Value>0x0100517159DC631A6D4CBD397E03E431C0C6</Value>
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
			</View>";
		}

		public IEnumerable<NewsModel> Take10(IQueryable<NewsModel> source)
		{
			return source
				.Take(10);
		}

		public string Take10CamlQuery()
		{
			return @"<View>
			  <RowLimit>10</RowLimit>
			  <Query>
				<Where>
				  <Eq>
					<FieldRef Name='ContentTypeId' />
					<Value>0x0100517159DC631A6D4CBD397E03E431C0C6</Value>
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
			</View>";
		}

		public IEnumerable<NewsModel> WhereTake10Query(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"))
				.Take(10);
		}

		public string WhereTake10CamlQuery()
		{
			return @"<View>
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
					  <Value>0x0100517159DC631A6D4CBD397E03E431C0C6</Value>
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
			</View>";
		}

		public NewsModel WhereFirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.First(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"));
		}

		public string WhereFirstQuery()
		{
			return @"<View>
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
					  <Value>0x0100517159DC631A6D4CBD397E03E431C0C6</Value>
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
			</View>";
		}

		public NewsModel WhereLastQuery(IQueryable<NewsModel> source)
		{
			return source
				.Last(n => n.Title.Contains("lorem") || n.Description.Contains("DESCRIPTION"));
		}

		public string WhereLastCamlQuery()
		{
			return @"<View>
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
					  <Value>0x0100517159DC631A6D4CBD397E03E431C0C6</Value>
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
			</View>";
		}
	}
}