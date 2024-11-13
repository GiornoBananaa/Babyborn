using System.Collections.Generic;
using ClothesSystem;
using Core.InstallationSystem.DataLoadingSystem;
using ItemSystem;
using ItemSystem.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.InstallationSystem
{
    public class GameLifeTimeScope : LifetimeScope
    {
        [SerializeField] private ItemMenuView[] _itemMenuViews;
        [SerializeField] private ItemCategoryButton[] _itemCategoryButtons;
        
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
            #endregion
            
            #region UI
            builder.RegisterComponent(_itemMenuViews).As<IEnumerable<ItemMenuView>>();
            builder.RegisterComponent(_itemCategoryButtons).As<IEnumerable<ItemCategoryButton>>();
            
            //builder.RegisterComponent(_itemMenuViews.AsEnumerable());
            #endregion
            
            #region Input
            //builder.RegisterComponent<InputListener>(_inputListener);
            #endregion

            #region Core
            //builder.Register<Game>(Lifetime.Singleton);
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
