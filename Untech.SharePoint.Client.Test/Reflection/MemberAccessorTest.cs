using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Reflection;
using Getter = System.Func<object, object>;

namespace Untech.SharePoint.Client.Test.Reflection
{
	[TestClass]
	public class MemberAccessorTest
	{
		public class Model
		{
			private string _field;

			public Model(string field)
			{
				_field = field;
			}

			public string Property
			{
				get { return _field; }
				set { _field = value; }
			}

			public string GetOnly
			{
				get { return _field; }
			}

			public string SetOnly
			{
				set { _field = value; }
			}
		}

		public class DerivedModel : Model
		{
			public DerivedModel(string field) : base(field)
			{
			}
		}

		[TestMethod]
		public void CanGetPublicProperty()
		{
			var accessor = new MemberAccessor();
			accessor.Initialize(typeof (Model));

			Assert.AreEqual(accessor[new Model("field"), "Property"], "field");
		}

		[TestMethod]
		public void CanSetPublicProperty()
		{
			var accessor = new MemberAccessor();
			accessor.Initialize(typeof (Model));

			var obj = new Model("test");
			accessor[obj, "Property"] = "updated";

			Assert.AreEqual(obj.Property, "updated");
		}

		[TestMethod]
		public void CanGetPrivateField()
		{
			var accessor = new MemberAccessor();
			accessor.Initialize(typeof(Model));

			Assert.AreEqual(accessor[new Model("field"), "_field"], "field");
		}

		[TestMethod]
		public void CanSetPrivateField()
		{
			var accessor = new MemberAccessor();
			accessor.Initialize(typeof (Model));

			var obj = new Model("test");
			accessor[obj, "_field"] = "updated";

			Assert.AreEqual(obj.Property, "updated");
		}


		[TestMethod]
		public void CanUseBaseClassProperty()
		{
			var accessor = new MemberAccessor();
			accessor.Initialize(typeof (DerivedModel));

			Assert.AreEqual(accessor[new DerivedModel("field"), "Property"], "field");
		}

		[TestMethod]
		public void ThrowIfNoGetter()
		{
			try
			{
				var accessor = new MemberAccessor();
				accessor.Initialize(typeof (Model));

				var obj = new Model("test");
				var test = accessor[obj, "SetOnly"];
				Assert.Fail();
			}
			catch (ArgumentException)
			{

			}
		}


		[TestMethod]
		public void ThrowIfNoSetter()
		{
			try
			{
				var accessor = new MemberAccessor();
				accessor.Initialize(typeof (Model));

				var obj = new Model("test");
				accessor[obj, "GetOnly"] = "new";
				Assert.Fail();
			}
			catch (ArgumentException)
			{

			}
		}

		[TestMethod]
		public void ThrowIfWrongObject()
		{
			try
			{
				var accessor = new MemberAccessor();
				accessor.Initialize(typeof (Model));

				var test = accessor["Wrong Object", "Property"];
				Assert.Fail();
			}
			catch (InvalidCastException)
			{

			}
		}

		[TestMethod]
		public void ThrowIfWrongPropertyValue()
		{
			try
			{
				var accessor = new MemberAccessor();
				accessor.Initialize(typeof (Model));

				accessor[new Model("test"), "Property"] = 10;
				Assert.Fail();
			}
			catch (InvalidCastException)
			{

			}
		}

	}
}

