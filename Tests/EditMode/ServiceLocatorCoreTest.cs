using NUnit.Framework;

namespace UniServiceLocator.Tests.EditMode
{
    public class EditorTest
    {
        [Test]
        public void ResolveTest1()
        { 
            var locator = new ServiceLocator();
            locator.Register<TestClass1>();
            
            Assert.That(locator.Resolve<TestClass1>() != null,Is.True);
        }
        
        [Test]
        public void ResolveTest2()
        { 
            var locator = new ServiceLocator();
            var test = new TestClass1();
            locator.Register(test);
            
            Assert.That(locator.Resolve<TestClass1>(),Is.EqualTo(test));
        }
        
        [Test]
        public void ResolveTest3()
        { 
            var locator = new ServiceLocator();
            locator.Register<TestClass1,ITestInterface1>();
            
            Assert.That(locator.Resolve<ITestInterface1>() != null,Is.True);
        }
    }
    
    class TestClass1 : ITestInterface1{}

    interface ITestInterface1{}
}

