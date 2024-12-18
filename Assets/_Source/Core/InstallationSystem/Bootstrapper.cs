using ItemSystem;
using LocationSystem;
using VContainer;
using VContainer.Unity;

namespace Core.InstallationSystem
{
    public class Bootstrapper : IStartable
    {
        private readonly ItemSaver _itemSaver;
        private readonly DollStatusSaver _dollStatusSaver;
        [Inject]
        public Bootstrapper(DollStatusSaver dollStatusSaver, ItemSaver itemSaver, LocationTransition locationTransition)
        {
            _itemSaver = itemSaver;
            _dollStatusSaver = dollStatusSaver;
        }

        void IStartable.Start()
        {
            _dollStatusSaver.LoadStatus();
            _itemSaver.LoadAllItems();
        }
    }
}
