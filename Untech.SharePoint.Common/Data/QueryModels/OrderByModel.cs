namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class OrderByModel
	{
		public OrderByModel(FieldRefModel field, bool ascending)
		{
			Field = field;
			Ascending = ascending;
		}

		public bool Ascending { get; set; }

		public FieldRefModel Field { get; set; }

		public OrderByModel Reverse()
		{
			return new OrderByModel(Field, !Ascending);
		}
	}
}