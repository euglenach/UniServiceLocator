using System;
using System.Collections.Generic;

namespace UniServiceLocator
{
    /// <summary>
    /// サービスロケーター
    /// </summary>
    public partial class ServiceLocator : IServiceLocator
    {
        public static readonly ServiceLocator Default = new();
        
        private readonly Dictionary<Type, ServiceObject> instanceContainer = new();

        public void Register<T>(Bind mode = Bind.Single) where T : class
        {
            var instance = mode switch
            {
                Bind.Single => Activator.CreateInstance<T>(),
                Bind.Transient => null,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            instanceContainer[typeof(T)] = new ServiceObject(instance,mode);
        }

        public void Register(Type type, Bind mode = Bind.Single)
        {
            var instance = mode switch
            {
                Bind.Single => Activator.CreateInstance(type),
                Bind.Transient => null,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            instanceContainer[type] = new ServiceObject(instance,mode);
        }

        public void Register<T>(T instance) where T : class
        {
            instanceContainer[typeof(T)] = new ServiceObject(instance,Bind.Single);
        }

        public void Register(Type type, object instance)
        {
            instanceContainer[type] = new ServiceObject(instance,Bind.Single);
        }

        public void Register<TClass, TInterface>(Bind mode = Bind.Single) where TClass : class
        {
            var instance = mode switch
            {
                Bind.Single => Activator.CreateInstance<TClass>(),
                Bind.Transient => null,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            instanceContainer[typeof(TInterface)] = new ServiceObject(instance,mode);
        }
        public T Resolve<T>() where T : class
        {
            if(instanceContainer.TryGetValue(typeof(T), out var service))
            {
                return service.mode switch
                {
                    Bind.Single => service.@ref as T,
                    Bind.Transient => Activator.CreateInstance<T>(),
                    _ => default
                };
            }

            return default;
        }
        
        void IDisposable.Dispose()
        {
            foreach(var instance in instanceContainer)
            {
                if(instance.Value.@ref is IDisposable disposable) disposable.Dispose();
            }
            instanceContainer.Clear();
        }

        private readonly struct ServiceObject
        {
            public readonly object @ref;
            public readonly Bind mode;

            public ServiceObject(object @ref, Bind mode)
            {
                this.@ref = @ref;
                this.mode = mode;
            }
        }
    }

    public interface IServiceLocator : IServiceLocatorRegister,IServiceLocatorResolver, IDisposable
    {
    }

    public partial interface IServiceLocatorRegister
    {
        void Register<T>(Bind mode = Bind.Single) where T : class;
        void Register(Type type, Bind mode = Bind.Single);
        void Register<T>(T instance) where T : class;
        void Register(Type type, object instance);
        void Register<TClass, TInterface>(Bind mode = Bind.Single) where TClass : class;
    }

    public interface IServiceLocatorResolver
    {
        T Resolve<T>() where T : class;
    }

    /// <summary>
    /// バインド方法
    /// </summary>
    public enum Bind
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        Single,

        /// <summary>
        /// 解決するとき都度インスタンスを作る
        /// </summary>
        Transient
    }
}
