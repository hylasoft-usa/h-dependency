using System;
using System.Collections;
using Hylasoft.Behavior;
using Hylasoft.Behavior.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hylasoft.Dependency.Tests
{
  [TestClass]
  public class AccountTest : Spec
  {
    [TestMethod]
    public void ShouldInitializeTheSingletonCorrectly()
    {
      // Init to true
      var dep = Hdependency.Initialize(true);
      Expect(dep).ToBe(Hdependency.Provider);
      Hdependency.Initialize(); //it shouldn't fail this time, since it's in test
      Expect(dep).ToNotBe(Hdependency.Provider);
      //It should fail now because it has been not initialized in test mode before
      Expect<Action>(() => Hdependency.Initialize()).ToThrowException<InvalidOperationException>();
    }

    [TestMethod]
    public void ShouldGetTheRightService()
    {
      var dep = new Hdependency();
      IEnumerable service = "test"; //a string is a valid IEnumerable, so this is possible
      dep.Register<IEnumerable>(service);
      Expect(dep.Get<IEnumerable>()).ToBe("test");
    }

    [TestMethod]
    public void ShouldntRegisterTwice()
    {
      var dep = new Hdependency();
      IEnumerable service = "test"; //a string is a valid IEnumerable, so this is possible
      dep.Register<IEnumerable>(service);
      // already registered
      Expect<Action>(() => dep.Register<IEnumerable>(service)).ToThrowException<InvalidOperationException>();
    }

    [TestMethod]
    public void ShouldntRegisterWrongInterface()
    {
      var dep = new Hdependency();
      var service = new object();
      // wrong interface
      Expect<Action>(() => dep.Register<IEnumerable>(service)).ToThrowException<ArgumentException>();
    }

    [TestMethod]
    public void ShouldThrowArgumentExceptionWhenNoService()
    {
      var dep = new Hdependency();
      // no such service
      Expect<Action>(() => dep.Get<IEnumerable>()).ToThrowException<ArgumentException>();
    }
  }
}
