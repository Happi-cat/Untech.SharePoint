using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Test
{
	public static class CustomAssert
	{
		public static void Throw<TException>(Action action)
			where TException: Exception
		{
			var thrown = false;
			try
			{
				action();
			}
			catch (TException)
			{
				thrown = true;
			}
			Assert.IsTrue(thrown);
		}
	}
}