using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Utils.Reflection;

namespace Untech.SharePoint.Client.Test.Reflection
{
	[TestClass]
	public class InstanceCreationFactoryTest
	{
		public class ModelOne
		{
			 
		}

		public class ModelTwo
		{
			 
		}

		public class ModelThree
		{

		}

		[TestMethod]
		public void CanRegisterTypes()
		{
			var factory = new InstanceCreationFactory<object>();

			factory.Register(typeof(ModelOne));
			factory.Register(typeof(ModelTwo));
			factory.Register(typeof(ModelThree));
		}


		[TestMethod]
		public void CanRegisterAndCreateType()
		{
			var factory = new InstanceCreationFactory<object>();

			factory.Register(typeof(ModelOne));

			var obj = factory.Create(typeof (ModelOne));
			
			Assert.IsNotNull(obj);
			Assert.IsInstanceOfType(obj, typeof(ModelOne));
		}

		[TestMethod]
		public void ThrowIfNotDerived()
		{
			try
			{
				var factory = new InstanceCreationFactory<ModelOne>();
				factory.Register(typeof(ModelTwo));
				Assert.Fail();
			}
			catch (ArgumentException)
			{

			}
		}

		[TestMethod]
		public void ThrowIfMissing()
		{
			try
			{
				var factory = new InstanceCreationFactory<object>();
				var obj = factory.Create(typeof (ModelOne));
				Assert.Fail();
			}
			catch (KeyNotFoundException)
			{
				
			}
		}
	}
}
