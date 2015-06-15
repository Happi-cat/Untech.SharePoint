using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Core.Reflection;

namespace Untech.SharePoint.Core.Test.Reflection
{
	[TestClass]
	public class PropertyAccessorTest
	{
		public class TestClass
		{
			private string _hiddenField;

			private string HiddenProp { get; set; }

			public string PropertyWithPrivateSetter { get; private set; }

			public string Property { get; set; }

			public string PropertyThrowException
			{
				get
				{
					throw new NotImplementedException();
				}
				set
				{
					throw new NotImplementedException();
				}
			}


			public string GetHiddenField()
			{
				return _hiddenField;
			}

			public string GetHiddenProp()
			{
				return HiddenProp;
			}
		}

		public PropertyAccessorTest()
		{
			TestClassAccessor = new PropertyAccessor();
			TestClassAccessor.Initialize(typeof(TestClass));
		}

		private PropertyAccessor TestClassAccessor { get; set; }

		[TestMethod]
		public void CanAccessPrivateField()
		{
			var testObj = new TestClass();

			TestClassAccessor[testObj, "_hiddenField"] = "TEST";

			Assert.AreEqual("TEST", testObj.GetHiddenField());
			Assert.AreEqual("TEST", TestClassAccessor[testObj, "_hiddenField"]);
		}

		[TestMethod]
		public void CanAccessPrivateProp()
		{
			var testObj = new TestClass();

			TestClassAccessor[testObj, "HiddenProp"] = "TEST";

			Assert.AreEqual("TEST", testObj.GetHiddenProp());
			Assert.AreEqual("TEST", TestClassAccessor[testObj, "HiddenProp"]);
		}

		[TestMethod]
		public void CanAccessPropWithPrivateSetter()
		{
			var testObj = new TestClass();

			TestClassAccessor[testObj, "PropertyWithPrivateSetter"] = "TEST";

			Assert.AreEqual("TEST", testObj.PropertyWithPrivateSetter);
		}

		[TestMethod]
		public void CanAccessProp()
		{
			var testObj = new TestClass();

			TestClassAccessor[testObj, "Property"] = "TEST";

			Assert.AreEqual("TEST", testObj.Property);
		}

		[TestMethod]
		public void PropTrowException()
		{
			var testObj = new TestClass();

			try
			{
				TestClassAccessor[testObj, "PropertyThrowException"] = "TEST";
			}
			catch (NotImplementedException)
			{
			}

			try
			{
				var a = TestClassAccessor[testObj, "PropertyThrowException"];
			}
			catch (NotImplementedException)
			{
			}
		}
	}
}
