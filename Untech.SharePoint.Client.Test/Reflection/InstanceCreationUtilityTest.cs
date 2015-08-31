using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Utils.Reflection;

namespace Untech.SharePoint.Client.Test.Reflection
{
	[TestClass]
	public class InstanceCreationUtilityTest
	{
		public class BaseClass
		{
			public BaseClass()
			{
				
			}

			public BaseClass(string prop1)
			{
				Prop1 = prop1;
			}

			public BaseClass(string prop1, string prop2)
			{
				Prop1 = prop1;
				Prop2 = prop2;
			}

			public BaseClass(string prop1, int prop3)
			{
				Prop1 = prop1;
				Prop3 = prop3;
			}

			public BaseClass(string prop1, string prop2, int prop3)
			{
				Prop1 = prop1;
				Prop2 = prop2;
				Prop3 = prop3;
			}


			public string Prop1 { get; set; }
			public string Prop2 { get; set; }

			public int Prop3 { get; set; }
		}

		public class DerivedFromBase : BaseClass
		{
			 
		}

		public class NonDerivedFromBase
		{
			 
		}

		[TestMethod]
		public void CanGetDefaultCreator()
		{
			var creator = InstanceCreationUtility.GetCreator<BaseClass>(typeof (BaseClass));
			var obj = creator();

			Assert.IsNotNull(obj);
		}

		[TestMethod]
		public void CanGetOneArgsCreator()
		{
			var creator = InstanceCreationUtility.GetCreator<string, BaseClass>(typeof(BaseClass));
			var obj = creator("1");

			Assert.IsNotNull(obj);
			Assert.AreEqual(obj.Prop1, "1");
			Assert.AreEqual(obj.Prop2, null);
			Assert.AreEqual(obj.Prop3, 0);
		}

		[TestMethod]
		public void CanGetTwoArgsCreator()
		{
			var creator1 = InstanceCreationUtility.GetCreator<string, string, BaseClass>(typeof(BaseClass));
			var creator2 = InstanceCreationUtility.GetCreator<string, int, BaseClass>(typeof(BaseClass));

			var obj1 = creator1("1", "2");

			Assert.IsNotNull(obj1);
			Assert.AreEqual(obj1.Prop1, "1");
			Assert.AreEqual(obj1.Prop2, "2");
			Assert.AreEqual(obj1.Prop3, 0);

			var obj2 = creator2("1", 2);

			Assert.IsNotNull(obj2);
			Assert.AreEqual(obj2.Prop1, "1");
			Assert.AreEqual(obj2.Prop2, null);
			Assert.AreEqual(obj2.Prop3, 2);
		}

		[TestMethod]
		public void CanGetThreeArgsCreator()
		{
			var creator = InstanceCreationUtility.GetCreator<string, string, int, BaseClass>(typeof(BaseClass));

			var obj = creator("1", "2", 3);

			Assert.IsNotNull(obj);
			Assert.AreEqual(obj.Prop1, "1");
			Assert.AreEqual(obj.Prop2, "2");
			Assert.AreEqual(obj.Prop3, 3);
		}

		[TestMethod]
		public void CanGetDerivedCreator()
		{
			var creator = InstanceCreationUtility.GetCreator<BaseClass>(typeof(DerivedFromBase));

			var obj = creator();

			Assert.IsNotNull(obj);
			Assert.IsInstanceOfType(obj, typeof(DerivedFromBase));
		}

		[TestMethod]
		public void ThrowIfNotDerived()
		{
			try
			{
				var creator = InstanceCreationUtility.GetCreator<BaseClass>(typeof (NonDerivedFromBase));
				Assert.Fail();
			}
			catch(ArgumentException)
			{
				
			}
		}

		[TestMethod]
		public void ThrowIfNoCreator()
		{
			try
			{
				var creator = InstanceCreationUtility.GetCreator<string, BaseClass>(typeof(DerivedFromBase));
				Assert.Fail();
			}
			catch (ArgumentException)
			{

			}
		}
	}

}
