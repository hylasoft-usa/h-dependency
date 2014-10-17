using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("h-dependency.test")]

namespace Hylasoft.Dependency
{
  /// <summary>
  /// Simple dependency mechanism provider to achieve loose coupling and easy mocking.
  /// It's a not a pure dependency injection provider,
  /// but it offers more flexibility in terms of where it can be used and initialized.
  /// It doesn't offer dependency resolution mechanism.
  /// </summary>
  public class Hdependency
  {
    #region static methods

    /// <summary>
    /// Initialize the provider. This method should be called during the bootstrapping of the application.
    /// </summary>
    /// <param name="test">If set to true, the provider can be reinitialized (default to false).
    /// It's useful when the provider needs to be used in unit tests, so it can be reinitialized</param>
    /// <exception cref="InvalidOperationException">This exception is thrown when the provider can't be instantiated,
    /// because it has already been instantiated in debug mode</exception>
    /// <returns>the newly created instance of the provider</returns>
    public static Hdependency Initialize(bool test = false)
    {
      if (!_canBeInitialized)
        throw new InvalidOperationException(
          "The dependency Initializer cannot be started twice if it's not initialized in test mode.");
      //set a new dependency injector.
      Provider = new Hdependency();
      _canBeInitialized = test;
      return Provider;
    }

    /// <summary>
    /// Gets the currently initialized provider. If the provider has not been initialized it returns null
    /// </summary>
    public static Hdependency Provider { get; private set; }

    private static bool _canBeInitialized = true;

    #endregion

    /// <summary>
    /// default constructor made private to enforce singleton pattern
    /// </summary>
    internal Hdependency()
    {
      _services = new Dictionary<Type, object>();
    }

    /// <summary>
    /// Dictornary containing all the registered service classes
    /// </summary>
    private readonly IDictionary<Type, object> _services;

    /// <summary>
    /// Register an instance in the provider
    /// </summary>
    /// <typeparam name="T">The type used to register the service.
    /// To retrieve the service, this instance must be used</typeparam>
    /// <param name="instance">The instance to be registered</param>
    /// <exception cref="ArgumentException">Thrown when the instance object is not an instance of type T</exception>
    /// <exception cref="InvalidOperationException">Thrown when a service registered with the same
    /// type already exists</exception>
    public void Register<T>(object instance) where T : class
    {
      if ((instance as T) == null)
        throw new ArgumentException("The instance has to be of the same class of the Type passed.");
      var type = typeof(T);
      if (_services.ContainsKey(type))
        throw new InvalidOperationException("The type has an already specified instance in the provider.");
      _services.Add(type, instance);
    }

    /// <summary>
    /// Get a service that is registered in the provider with the requested type
    /// </summary>
    /// <typeparam name="T">the type that the passed service is registered with</typeparam>
    /// <returns>The service that implements the requested type</returns>
    /// <exception cref="ArgumentException">Thrown when the provider doesn't contain a
    /// service for the type T</exception>
    public T Get<T>() where T : class
    {
      object val;
      if (!_services.TryGetValue(typeof(T), out val))
        throw new ArgumentException("Unable to find " + typeof(T).Name + ". It appears is not registered.");
      return val as T;
    }
  }
}