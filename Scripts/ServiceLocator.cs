using System;
using System.Collections.Generic;

namespace UniServiceLocator
{
    public class ServiceLocator : IServiceLocator
    {
        bool isDisposed;
        private readonly Dictionary<Type, ServiceObject> instanceContainer = new Dictionary<Type, ServiceObject>();

        public void Register<T>(Bind mode = Bind.Single)
        {
            var instance = mode switch
            {
                Bind.Single => Activator.CreateInstance<T>(),
                Bind.Transient => default,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            instanceContainer[typeof(T)] = new ServiceObject(new WeakReference<object>(instance),mode);
        }

        public void Register<T>(T instance)
        {
            instanceContainer[typeof(T)] = new ServiceObject(new WeakReference<object>(instance),Bind.Single);
        }

        public void Register<TClass, TInterface>(Bind mode = Bind.Single)
        {
            var instance = mode switch
            {
                Bind.Single => Activator.CreateInstance<TClass>(),
                Bind.Transient => default,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            instanceContainer[typeof(TInterface)] = new ServiceObject(new WeakReference<object>(instance),mode);
        }

        public T Resolve<T>()
        {
            if(instanceContainer.TryGetValue(typeof(T), out var service))
            {
                return service.mode switch
                {
                    Bind.Single => service.@ref.TryGetTarget(out var obj) ? (T)obj: default,
                    Bind.Transient => Activator.CreateInstance<T>(),
                    _ => default
                };
            }

            return default;
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposing || isDisposed) return;
            
            foreach(var instance in instanceContainer)
            {
                if(!instance.Value.@ref.TryGetTarget(out var obj)) continue;
                
                if(obj is IDisposable disposable) disposable.Dispose();
            }
            instanceContainer.Clear();
        }

        public void Dispose()
        {
            if(isDisposed) return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private readonly struct ServiceObject
        {
            public readonly WeakReference<object> @ref;
            public readonly Bind mode;

            public ServiceObject(WeakReference<object> @ref, Bind mode)
            {
                this.@ref = @ref;
                this.mode = mode;
            }
        }
    }
}
