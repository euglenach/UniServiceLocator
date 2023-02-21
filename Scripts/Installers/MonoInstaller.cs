using UnityEngine;

namespace UniServiceLocator.Installers
{
    public abstract class MonoInstaller : MonoBehaviour
    {
        public abstract void InstallBinding(IServiceLocatorRegister register);
    }
}
