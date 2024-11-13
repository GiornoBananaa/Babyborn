using System;
using System.Collections.Generic;
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

        public event Action<Item> OnItemClicked;
        
        [Inject]
        public void Construct(ItemContainer itemContainer)
        {
            _items = itemContainer.Get(Category);
            BuildList();
        }
        
        private void BuildList()
        {
            foreach (Item item in _items)
            {
                MenuItem menuItem = Instantiate(_itemPrefab, _itemListParent);
                menuItem.Construct(item);
                _menuItems.Add(menuItem);
                menuItem.UseButton.onClick.AddListener(() => OnItemClicked?.Invoke(item));
            }
        }
    }
}