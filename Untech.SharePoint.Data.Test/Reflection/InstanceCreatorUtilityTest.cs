using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Data.Reflection;

namespace Untech.SharePoint.Data.Test.Reflection
{
    [TestClass]
    public class InstanceCreatorUtilityTest
    {
        public class NonBaseClass
        {
             
        }

        public class BaseClass
        {
            
        }

        public interface IBaseInterface
        {
             
        }

        public class TestClass : BaseClass, IBaseInterface
        {
            public TestClass()
            {
                
            }

            public TestClass(string property)
            {
                Property1 = property;
            }

            public TestClass(string property1, string property2)
            {
                Property1 = property1;
                Property2 = property2;
            }

            public TestClass(string property1, string property2, string property3)
            {
                Property1 = property1;
                Property2 = property2;
                Property3 = property3;
            }

            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public string Property3 { get; set; }
        }

        [TestMethod]
        public void CreateWithoutArgs()
        {
            var etalon = new TestClass();
            var test = InstanceCreatorUtility.GetCreator<TestClass>(typeof(TestClass))();

            Assert.AreEqual(etalon.Property1, test.Property1);
            Assert.AreEqual(etalon.Property2, test.Property2);
            Assert.AreEqual(etalon.Property3, test.Property3);
        }

        [TestMethod]
        public void CreateWithOneArg()
        {
            var etalon = new TestClass("TEST");
            var test = InstanceCreatorUtility.GetCreator<string, TestClass>(typeof(TestClass))("TEST");

            Assert.AreEqual(etalon.Property1, test.Property1);
            Assert.AreEqual(etalon.Property2, test.Property2);
            Assert.AreEqual(etalon.Property3, test.Property3);
        }

        [TestMethod]
        public void CreateWithTwoArgs()
        {
            var etalon = new TestClass("ONE", "TWO");
            var test = InstanceCreatorUtility.GetCreator<string, string, TestClass>(typeof(TestClass))("ONE", "TWO");

            Assert.AreEqual(etalon.Property1, test.Property1);
            Assert.AreEqual(etalon.Property2, test.Property2);
            Assert.AreEqual(etalon.Property3, test.Property3);
        }

        [TestMethod]
        public void CreateWithThreeArgs()
        {
            var etalon = new TestClass("1", "2", "3");
            var test = InstanceCreatorUtility.GetCreator<string, string, string, TestClass>(typeof(TestClass))("1", "2", "3");

            Assert.AreEqual(etalon.Property1, test.Property1);
            Assert.AreEqual(etalon.Property2, test.Property2);
            Assert.AreEqual(etalon.Property3, test.Property3);
        }

        [TestMethod]
        public void CreateWithBaseClass()
        {
            var test = InstanceCreatorUtility.GetCreator<BaseClass>(typeof(TestClass))();
        }

        [TestMethod]
        public void CreateWithInterface()
        {
            var test = InstanceCreatorUtility.GetCreator<IBaseInterface>(typeof(TestClass))();
        }

        [TestMethod]
        public void ThrowArgumentException()
        {
            try
            {
                var test = InstanceCreatorUtility.GetCreator<NonBaseClass>(typeof (TestClass))();
            }
            catch (ArgumentException)
            {

            }
            catch (Exception)
            {
                Assert.Fail();
            }
            
        }
    }
}
