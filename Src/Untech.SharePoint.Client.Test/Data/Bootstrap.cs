﻿using Untech.SharePoint.Client.Configuration;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Test.Data
{
	public class Bootstrap
	{
		public Bootstrap()
		{
			Config = ClientConfig.Begin()
				.RegisterMappings(n => n.Annotated<DataContext>())
				.BuildConfig();
		}

		private Config Config { get; }

		public static Config GetConfig()
		{
			return Singleton<Bootstrap>.GetInstance().Config;
		}
	}
}