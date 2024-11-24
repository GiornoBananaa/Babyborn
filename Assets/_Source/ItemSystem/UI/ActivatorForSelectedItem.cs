using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace ItemSystem.UI
{
    
    public class ActivatorForSelectedItem : MonoBehaviour
    {
        [Serializable]
        public class ItemActivationObjects
        {
            public ItemDataSO ItemData;
            public Behaviour[] Objects;
        }
        
        [SerializeField] public ItemActivationObjects[] _sceneItems;
        
        private Dictionary<int, Behaviour[]> _items;
        private Item _item;
        private ItemSelector _itemSelector;
        
        [Inject]
        public void Construct(ItemSelector itemSelector)
        {
            _itemSelector = itemSelector;
            _itemSelector.OnItemSelected += OnSelect;
            _itemSelector.OnItemUnselected += OnUnselect;
            _items = _sceneItems.ToDictionary(
                activation => activation.ItemData.GetInstanceID(), 
                activation => activation.Objects);
        }

        private void OnSelect(Item item)
        {
            if(_items.ContainsKey(item.ID)) return;

            foreach (var obj in _items[item.ID])
            {
                obj.enabled = true;
            }
            
            _item = item;
        }
        
        private void OnUnselect(Item item)
        {
            if(_item != item) return;
            
            foreach (var obj in _items[item.ID])
            {
                obj.enabled = false;
            }
            
            _item = null;
        }
        
        private void OnDestroy()
        {
            _itemSelector.OnItemSelected -= OnSelect;
        }
    }
}