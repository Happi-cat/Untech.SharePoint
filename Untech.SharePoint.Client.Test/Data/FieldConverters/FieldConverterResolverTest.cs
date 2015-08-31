using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.FieldConverters;

namespace Untech.SharePoint.Client.Test.Data.FieldConverters
{
	[TestClass]
	public class FieldConverterResolverTest
	{
		public class CustomConverter : IFieldConverter
		{
			public void Initialize(Field field, Type propertyType)
			{
				throw new NotImplementedException();
			}

			public object FromSpClientValue(object value)
			{
				throw new NotImplementedException();
			}

			public object ToSpClientValue(object value)
			{
				throw new NotImplementedException();
			}

			public string ToCamlValue(object value)
			{
				throw new NotImplementedException();
			}
		}


		[TestMethod]
		public void CanRegisterBuiltInConverters()
		{
			var resolver = new FieldConverterResolver();

			FieldConverterResolver.RegisterBuiltInConverters(resolver);
		}

		[TestMethod]
		public void CanRegisterCustomConverters()
		{
			var resolver = new FieldConverterResolver();

			resolver.Register(typeof(CustomConverter));
		}

		[TestMethod]
		public void CanCreateBuiltInConverters()
		{
			var resolver = new FieldConverterResolver();

			FieldConverterResolver.RegisterBuiltInConverters(resolver);

			var instance = resolver.Create("Counter");

			Assert.IsNotNull(instance);
		}

		[TestMethod]
		public void CanCreateCustomConverters()
		{
			var resolver = new FieldConverterResolver();

			resolver.Register(typeof(CustomConverter));

			var instance = resolver.Create(typeof(CustomConverter));

			Assert.IsNotNull(instance);
		}


		[TestMethod]
		public void ThrowIfInvalidConverter()
		{
			try
			{
				var resolver = new FieldConverterResolver();

				resolver.Register(typeof (object));
				Assert.Fail();
			}
			catch (FieldConverterException)
			{
				
			}
		}

		[TestMethod]
		public void ThrowIfNotFound()
		{
			try
			{
				var resolver = new FieldConverterResolver();

				resolver.Create("NotFound");
				Assert.Fail();
			}
			catch (KeyNotFoundException)
			{

			}
		}
	}
}
