using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hylasoft.dependency.tests
{

  namespace Bank
  {

    [TestClass]
    public class AccountTest
    {
      [TestMethod]
      public void ShouldInitializeTheSingletonCorrectly()
      {
        // Init to true
        var dep = HDependency.Initialize(true);
        Assert.AreEqual(dep, HDependency.Provider, "the returned hdep should be the same of the singleton");
        try
        {
          HDependency.Initialize();
        }
        catch (Exception)
        {
          Assert.Fail("it shouldn't fail this time, since it's in test");
        }
        Assert.AreNotEqual(dep, HDependency.Provider, "the two deps now shouldn't be the same, since it's been reinitialized");
        try
        {
          HDependency.Initialize(true);
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
        var dep = new HDependency();
        IEnumerable service = "test"; //a string is a valid IEnumerable, so this is possible
        dep.Register<IEnumerable>(service);
        Assert.AreEqual("test", dep.Get<IEnumerable>());
      }

      [TestMethod]
      public void ShouldntRegisterTwice()
      {
        var dep = new HDependency();
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
        var dep = new HDependency();
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
        var dep = new HDependency();
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
}