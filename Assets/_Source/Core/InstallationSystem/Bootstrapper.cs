using ItemSystem;
using VContainer;
using VContainer.Unity;

namespace Core.InstallationSystem
{
    public class Bootstrapper : IStartable
    {
        [Inject]
        public Bootstrapper(ItemSelector itemSelector)
        {
        
        }

        void IStartable.Start()
        {
        
        }
    }
}
