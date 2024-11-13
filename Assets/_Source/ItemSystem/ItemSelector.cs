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
        private readonly Dictionary<ItemCategory, int> _maxSelectionsCounts;
        private readonly Dictionary<ItemCategory, bool> _unselectOldSelections;

        public event Action<Item> OnItemSelected;
        
        public ItemSelector(IEnumerable<ItemMenuView> itemMenus, IRepository<ScriptableObject> repository)
        {
            _maxSelectionsCounts = repository.GetItem<ItemCategoryConfigSO>().ToDictionary((config) => config.Category, (config) => config.MaxSelectedCount);
            _unselectOldSelections = repository.GetItem<ItemCategoryConfigSO>().ToDictionary((config) => config.Category, (config) => config.UnselectOldSelection);
            foreach (var itemMenu in itemMenus)
            {
                itemMenu.OnItemClicked += OnItemClicked;
            }
        }

        private void OnItemClicked(Item item)
        {
            if (!_selectedItems.ContainsKey(item.Category))
            {
                _selectedItems.Add(item.Category, new HashSet<Item>());
            }
            
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
                Select(item);
            }
        }
        
        public void Select(Item item)
        {
            _selectedItems[item.Category].Add(item);
            item.Selected.Value = true;
            OnItemSelected?.Invoke(item);
        }

        private void Unselect(Item item)
        {
            if(!_selectedItems.ContainsKey(item.Category)
            || !_selectedItems[item.Category].Contains(item)) return;
            _selectedItems[item.Category].Remove(item);
            item.Selected.Value = false;
        }
    }
}
