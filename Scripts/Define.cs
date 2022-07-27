using System;

namespace UniServiceLocator
{
    public interface ILocator : IDisposable
    {
        void Register<T>(Bind mode = Bind.Single);
        void Register<T>(T instance);
        void Register<TClass, TInterface>(Bind mode = Bind.Single);
        T Resolve<T>();
    }
    
    public enum Bind
    {
        Single,
        Transient
    }
}
