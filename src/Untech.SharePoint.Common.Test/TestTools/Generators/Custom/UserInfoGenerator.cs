using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.Models;
using Untech.SharePoint.TestTools.Generators.Basic;

namespace Untech.SharePoint.TestTools.Generators.Custom
{
	public class UserInfoGenerator : BaseRandomGenerator, IValueGenerator<UserInfo>
	{
		[NotNull]
		private readonly IReadOnlyList<int> _userIds;

		public UserInfoGenerator([CanBeNull]IEnumerable<int> userIds)
		{
			_userIds = userIds.EmptyIfNull().ToList();
		}

		public UserInfo Generate()
		{
			return new UserInfo
			{
				Id = _userIds[Rand.Next(_userIds.Count)]
			};
		}
	}
}