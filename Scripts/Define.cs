using System;

namespace UniServiceLocator
{
    public interface IServiceLocator : IRegister, IResolver, IDisposable
    {}

    public interface IRegister
    {
        void Register<T>(Bind mode = Bind.Single);
        void Register<T>(T instance);
        void Register<TClass, TInterface>(Bind mode = Bind.Single);
    }

    public interface IResolver
    {
        T Resolve<T>();
    }
    
    public enum Bind
    {
        /// <summary>
        /// Generate a unique instance on registration.
        /// </summary>
        Single,
        
        /// <summary>
        /// instantiate on resolution.
        /// </summary>
        Transient
    }
}
