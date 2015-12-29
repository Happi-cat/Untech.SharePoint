using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Test.Tools.Generators
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