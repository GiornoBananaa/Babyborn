using System;
using System.Collections.Generic;
using System.Linq;
using ClothesSystem;
using Core.InstallationSystem.DataLoadingSystem;
using UnityEngine;
using VContainer;

namespace ItemSystem.UI
{
    public class ItemMenuView : MonoBehaviour
    {
        //TODO: ItemMenuView -> ICategoryObject
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [SerializeField] private RectTransform _panel;
        [SerializeField] private RectTransform _itemListParent;
        [SerializeField] private MenuItem _itemPrefab;
        
        private readonly List<MenuItem> _menuItems = new();
        private Item[] _items;
        private Dictionary<ItemCategory, ItemCategoryConfigSO> _categoryConfigs;

        public event Action<Item> OnItemClicked;
        
        [Inject]
        public void Construct(ItemContainer itemContainer, IRepository<ScriptableObject> repository)
        {
            _categoryConfigs = repository.GetItem<ItemCategoryConfigSO>().ToDictionary(config => config.Category);
            _items = itemContainer.Get(Category);
            BuildList();
        }
        
        private void BuildList()
        {
            foreach (Item item in _items)
            {
                MenuItem menuItem = Instantiate(_itemPrefab, _itemListParent);
                menuItem.Construct(item, _categoryConfigs[item.Category].SelectByPointerUp);
                _menuItems.Add(menuItem);
                menuItem.OnSelected.AddListener(() => OnItemClicked?.Invoke(item));
            }
        }
    }
}