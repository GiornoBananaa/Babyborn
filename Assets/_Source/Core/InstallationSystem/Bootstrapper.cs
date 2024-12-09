using ItemSystem;
using LocationSystem;
using VContainer;
using VContainer.Unity;

namespace Core.InstallationSystem
{
    public class Bootstrapper : IStartable
    {
        private readonly ItemSaver _itemSaver;
        [Inject]
        public Bootstrapper(ItemSaver itemSaver, LocationTransition locationTransition)
        {
            _itemSaver = itemSaver;
        }

        void IStartable.Start()
        {
            _itemSaver.LoadAllItems();
        }
    }
}
