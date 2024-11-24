using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemSystem.UI
{
    public class ActivatorForCategorySelection : MonoBehaviour
    {
        [Serializable]
        public class ActivationObjectsByCategory
        {
            public ItemCategory ItemCategory;
            public Behaviour[] Objects;
        }
        
        [SerializeField] public CategoryMenuSelector _menuSelector;
        [SerializeField] public ActivationObjectsByCategory[] _sceneItems;
        
        private Dictionary<ItemCategory, Behaviour[]> _items;
        private Item _item;
        
        public void Awake()
        {
            _menuSelector.OnCategorySelected += OnSelect;
            _menuSelector.OnCategoryUnselected += OnUnselect;
            _items = _sceneItems.ToDictionary(
                activation => activation.ItemCategory, 
                activation => activation.Objects);
        }

        private void OnSelect(ItemCategory item)
        {
            if(!_items.TryGetValue(item, out var behaviours)) return;
            
            foreach (var obj in behaviours)
            {
                obj.enabled = true;
            }
        }
        
        private void OnUnselect(ItemCategory item)
        {
            if(!_items.TryGetValue(item, out var behaviours)) return;
            
            foreach (var obj in behaviours)
            {
                obj.enabled = false;
            }
        }
        
        private void OnDestroy()
        {
            _menuSelector.OnCategorySelected -= OnSelect;
            _menuSelector.OnCategoryUnselected -= OnUnselect;
        }
    }
}