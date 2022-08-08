using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UniServiceLocator.Tests.PlayMode
{
    public class ProjectContextTest
    {
        [UnityTest]
        public IEnumerator GenerateProjectContextTest()
        {
            ProjectContext.Locator.Register<TestClass1>();
            yield return null;
            var projectContext = Object.FindObjectOfType<ProjectContext>();
            Assert.That(projectContext != null,Is.True);
        }
        
        [UnityTest]
        public IEnumerator ResolveProjectContextTest()
        {
            var test = new TestClass1();
            yield return null;
            ProjectContext.Locator.Register<ITestInterFace1>(test);
            Assert.That(ProjectContext.Locator.Resolve<ITestInterFace1>() != null,Is.True);
        }
    }
    
    class TestClass1 : ITestInterFace1{}
    interface ITestInterFace1{}
}
