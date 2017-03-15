using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Converters
{
	[TestClass]
	public class FieldConverterContainerTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddType_ThrowArgumentNull_WhenTypeIsNull()
		{
			var container = new FieldConverterContainer();
			container.Add(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddType_ThrowArgument_WhenTypeIsNotAFieldConverterNull()
		{
			var container = new FieldConverterContainer();
			container.Add(GetType());
		}

		[TestMethod]
		public void AddType_Works_WhenCalledTwice()
		{
			var container = new FieldConverterContainer();
			container.Add(typeof(BuiltInTestFieldConverter));
			container.Add(typeof(BuiltInTestFieldConverter));
		}

		[TestMethod]
		public void AddT_Works()
		{
			var container = new FieldConverterContainer();
			container.Add<BuiltInTestFieldConverter>();
		}

		[TestMethod]
		public void AddFromAssembly_Works()
		{
			var container = new FieldConverterContainer();
			container.AddFromAssembly(GetType().Assembly);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddFromAssembly_ThrowArgumentNull_WhenAssemblyIsNull()
		{
			var container = new FieldConverterContainer();
			container.AddFromAssembly(null);
		}

		[TestMethod]
		public void ResolveByName_Resolves_WhenRegisteredFromAssembly()
		{
			var container = new FieldConverterContainer();
			container.AddFromAssembly(GetType().Assembly);

			Assert.IsNotNull(container.Resolve("BUILT_IN_TEST_CONVERTER"));
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void ResolveByName_ThrowKeyNotFound_WhenRegisteredByType()
		{
			var container = new FieldConverterContainer();
			container.Add<BuiltInTestFieldConverter>();

			container.Resolve("BUILT_IN_TEST_CONVERTER");
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void ResolveByName_ThrowKeyNotFound_WhenNotRegisterd()
		{
			var container = new FieldConverterContainer();
			container.Add<BuiltInTestFieldConverter>();

			container.Resolve("BUILT_IN_TEST_CONVERTER");
		}

		[TestMethod]
		public void CanResolveByName_ReturnsTrue_WhenRegisteredFromAssembly()
		{
			var container = new FieldConverterContainer();
			container.AddFromAssembly(GetType().Assembly);

			Assert.IsTrue(container.CanResolve("BUILT_IN_TEST_CONVERTER"));
		}

		[TestMethod]
		public void CanResolveByName_ReturnsFalse_WhenRegisteredByType()
		{
			var container = new FieldConverterContainer();
			container.Add<BuiltInTestFieldConverter>();

			Assert.IsFalse(container.CanResolve("BUILT_IN_TEST_CONVERTER"));
		}

		[TestMethod]
		public void CanResolveByName_ReturnsFalse_WhenNotRegisterd()
		{
			var container = new FieldConverterContainer();
			container.Add<BuiltInTestFieldConverter>();

			Assert.IsFalse(container.CanResolve("BUILT_IN_TEST_CONVERTER"));
		}

		[TestMethod]
		public void ResolveByType_Resolves_WhenRegistered()
		{
			var container = new FieldConverterContainer();
			container.Add<BuiltInTestFieldConverter>();

			Assert.IsNotNull(container.Resolve(typeof(BuiltInTestFieldConverter)));
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void ResolveByType_ThrowKeyNotFound_WhenNotRegistered()
		{
			var container = new FieldConverterContainer();

			container.Resolve(typeof(BuiltInTestFieldConverter));
		}

		[TestMethod]
		public void CanResolveByType_ReturnsTrue_WhenRegistered()
		{
			var container = new FieldConverterContainer();
			container.Add<BuiltInTestFieldConverter>();

			Assert.IsTrue(container.CanResolve(typeof(BuiltInTestFieldConverter)));
		}

		[TestMethod]
		public void CanResolveByType_ReturnsFalse_WhenNotRegistered()
		{
			var container = new FieldConverterContainer();

			Assert.IsFalse(container.CanResolve(typeof(BuiltInTestFieldConverter)));
		}
	}
}
