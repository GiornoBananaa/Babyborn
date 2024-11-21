using ItemSystem;
using LocationSystem;
using VContainer;
using VContainer.Unity;

namespace Core.InstallationSystem
{
    public class Bootstrapper : IStartable
    {
        [Inject]
        public Bootstrapper(ItemSelector itemSelector, LocationTransition locationTransition)
        {
        
        }

        void IStartable.Start()
        {
        
        }
    }
}
