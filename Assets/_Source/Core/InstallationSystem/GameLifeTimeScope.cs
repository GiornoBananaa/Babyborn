using System.Collections.Generic;
using ClothesSystem;
using Core.InstallationSystem.DataLoadingSystem;
using ItemSystem;
using ItemSystem.UI;
using LocationSystem;
using LocationSystem.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.InstallationSystem
{
    public class GameLifeTimeScope : LifetimeScope
    {
        [SerializeField] private ItemMenuView[] _itemMenuViews;
        [SerializeField] private SceneItem[] _sceneItems;
        [SerializeField] private ItemCategoryButton[] _itemCategoryButtons;
        [SerializeField] private TransitionButton[] _locationButtons;
        [SerializeField] private Location[] _locations;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<Bootstrapper>();

            #region DataLoad
            IResourceLoader resourceLoader = new ResourceLoader();
            IRepository<ScriptableObject> dataRepository = new DataRepository<ScriptableObject>();

            LoadResources(resourceLoader, dataRepository);
            
            builder.RegisterInstance(dataRepository);
            #endregion
            
            #region Item
            builder.Register<ItemContainer>(Lifetime.Singleton);
            builder.Register<ItemSelector>(Lifetime.Singleton);
            builder.Register<ItemSaver>(Lifetime.Singleton);
            
            builder.RegisterComponent(_itemMenuViews).As<IEnumerable<ItemMenuView>>();
            builder.RegisterComponent(_itemCategoryButtons).As<IEnumerable<ItemCategoryButton>>();
            builder.RegisterComponent(_sceneItems).As<IEnumerable<SceneItem>>();
            #endregion
            
            #region Location
            builder.Register<LocationTransition>(Lifetime.Singleton);
            builder.RegisterComponent(_locations).As<IEnumerable<Location>>();
            
            builder.RegisterComponent(_locationButtons).As<IEnumerable<TransitionButton>>();
            #endregion
        }

        private void LoadResources(IResourceLoader resourceLoader, IRepository<ScriptableObject> dataRepository)
        {
            resourceLoader.LoadResource(PathData.ITEM_DATA_PATH,
                typeof(ItemDataSO), dataRepository);
            resourceLoader.LoadResource(PathData.ITEM_CATEGORY_DATA_PATH,
                typeof(ItemCategoryConfigSO), dataRepository);
            
        }
    }
}
