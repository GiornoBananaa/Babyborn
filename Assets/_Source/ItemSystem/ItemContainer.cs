using System.Collections.Generic;
using System.Linq;
using Core.InstallationSystem.DataLoadingSystem;
using UnityEngine;

namespace ItemSystem
{
    public class ItemContainer
    {
        private readonly Dictionary<ItemCategory, HashSet<Item>> _items;
        
        public ItemContainer(IRepository<ScriptableObject> repository)
        {
            _items = new Dictionary<ItemCategory, HashSet<Item>>();
            foreach (ItemDataSO item in repository.GetItem<ItemDataSO>())
            {
                Add(item);
            }
        }

        public Item[] Get(ItemCategory category)
        {
            return _items[category].ToArray();
        }
    
        public Dictionary<ItemCategory, Item[]> GetAll()
        {
            Dictionary<ItemCategory, Item[]> items = new Dictionary<ItemCategory, Item[]>();
            foreach (var itemCategory in _items.Keys)
            {
                items.Add(itemCategory, _items[itemCategory].ToArray());
            }
            return items;
        }
    
        private void Add(ItemDataSO itemData)
        {
            if (!_items.ContainsKey(itemData.Category))
            {
                _items.Add(itemData.Category, new HashSet<Item>());
            }
            
            Item item = new Item(itemData.Sprite, itemData.Category, itemData.SpriteCenterOffset, itemData.AlignSizeByWidth, itemData.Unlocked);
            _items[item.Category].Add(item);
        }
    }
}