using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hylasoft.Dependency.Tests
{
  [TestClass]
  public class AccountTest
  {
    [TestMethod]
    public void ShouldInitializeTheSingletonCorrectly()
    {
      // Init to true
      var dep = Hdependency.Initialize(true);
      Assert.AreEqual(dep, Hdependency.Provider, "the returned hdep should be the same of the singleton");
      try
      {
        Hdependency.Initialize();
      }
      catch (Exception)
      {
        Assert.Fail("it shouldn't fail this time, since it's in test");
      }
      Assert.AreNotEqual(dep, Hdependency.Provider,
        "the two deps now shouldn't be the same, since it's been reinitialized");
      try
      {
        Hdependency.Initialize(true);
        Assert.Fail("It should fail now because it has been not initialized in test mode before");
      }
      catch (InvalidOperationException e)
      {
        //here is fine, the exception is being correctly thrown
      }
    }

    [TestMethod]
    public void ShouldGetTheRightService()
    {
      var dep = new Hdependency();
      IEnumerable service = "test"; //a string is a valid IEnumerable, so this is possible
      dep.Register<IEnumerable>(service);
      Assert.AreEqual("test", dep.Get<IEnumerable>());
    }

    [TestMethod]
    public void ShouldntRegisterTwice()
    {
      var dep = new Hdependency();
      IEnumerable service = "test"; //a string is a valid IEnumerable, so this is possible
      dep.Register<IEnumerable>(service);
      try
      {
        dep.Register<IEnumerable>(service); //already registered
        Assert.Fail();
      }
      catch (InvalidOperationException)
      {
        //here is fine, the exception is being correctly thrown
      }
    }

    [TestMethod]
    public void ShouldntRegisterWrongInterface()
    {
      var dep = new Hdependency();
      var service = new object();
      try
      {
        dep.Register<IEnumerable>(service); //wrong interface
        Assert.Fail();
      }
      catch (ArgumentException)
      {
        //here is fine, the exception is being correctly thrown
      }
    }

    [TestMethod]
    public void ShouldThrowArgumentExceptionWhenNoService()
    {
      var dep = new Hdependency();
      try
      {
        dep.Get<IEnumerable>(); //no such service
        Assert.Fail();
      }
      catch (ArgumentException)
      {
        //here is fine, the exception is being correctly thrown
      }
    }
  }
}