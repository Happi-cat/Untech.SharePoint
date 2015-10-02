using System.Reflection;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class FieldRefModel
	{
		public FieldRefModel(MemberInfo member)
		{
			Member = member;
		}

		public MemberInfo Member { get; set; }

		public override string ToString()
		{
			return string.Format("<FieldRef Name='{0}' />", Member.Name);
		}
	}
}