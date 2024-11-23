using System;
using System.Collections.Generic;
using System.Linq;
using ClothesSystem;
using Core.InstallationSystem.DataLoadingSystem;
using ItemSystem.UI;
using UnityEngine;
using VContainer;

namespace ItemSystem
{
    public class ItemSelector
    {
        private readonly Dictionary<ItemCategory, HashSet<Item>> _selectedItems = new();
        private readonly Dictionary<ItemCategory, int> _maxSelectionsCounts = new();
        private readonly Dictionary<ItemCategory, bool> _unselectOldSelections = new();
        private readonly Dictionary<ItemCategory, ItemCategory[]> _overlappingSelections = new();

        public event Action<Item> OnItemSelected;
        public event Action<Item> OnItemUnselected;
        
        public ItemSelector(IEnumerable<ItemMenuView> itemMenus, IEnumerable<SceneItem> sceneItems, IRepository<ScriptableObject> repository)
        {
            List<ItemCategoryConfigSO> categoryConfigs = repository.GetItem<ItemCategoryConfigSO>();
            foreach (var config in categoryConfigs)
            {
                if (!_selectedItems.ContainsKey(config.Category))
                {
                    _selectedItems.Add(config.Category, new HashSet<Item>());
                }
                _maxSelectionsCounts.Add(config.Category, config.MaxSelectedCount);
                _unselectOldSelections.Add(config.Category, config.UnselectOldSelection);
                _overlappingSelections.Add(config.Category, config.OverlappedCategories);
            }
            
            foreach (var itemMenu in itemMenus)
            {
                itemMenu.OnItemClicked += OnItemClicked;
            }
            
            foreach (var item in sceneItems)
            {
                item.OnClick += OnItemClicked;
            }
        }

        private void OnItemClicked(Item item)
        {
            if(_selectedItems.TryGetValue(item.Category, out var selected)
               && selected.Contains(item))
            {
                Unselect(item);
            }
            else
            {
                if(_unselectOldSelections[item.Category] 
                   && _selectedItems[item.Category].Count >= _maxSelectionsCounts[item.Category])
                    Unselect(_selectedItems[item.Category].First());
                
                foreach (var overlappedCategory in _overlappingSelections[item.Category])
                {
                    foreach (var overlappedItem in _selectedItems[overlappedCategory].ToArray())
                    {
                        Unselect(overlappedItem);
                    }
                }
                Select(item);
            }
        }
        
        public void Select(Item item)
        {
            _selectedItems[item.Category].Add(item);
            item.Selected.Value = true;
            OnItemSelected?.Invoke(item);
        }

        public void Unselect(Item item)
        {
            if(!_selectedItems.ContainsKey(item.Category)
            || !_selectedItems[item.Category].Contains(item)) return;
            _selectedItems[item.Category].Remove(item);
            item.Selected.Value = false;
            OnItemUnselected?.Invoke(item);
        }
    }
}
