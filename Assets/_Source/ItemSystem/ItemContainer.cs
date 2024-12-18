using System;
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
            return _items.TryGetValue(category, out var item) ? item.ToArray() : Array.Empty<Item>();
        }
        
        public Item Get(ItemCategory category, int id)
        {
            return _items[category].First(item => item.ID == id);
        }
        
        public Dictionary<ItemCategory, Item[]> GetAllByCategory()
        {
            Dictionary<ItemCategory, Item[]> items = new Dictionary<ItemCategory, Item[]>();
            foreach (var itemCategory in _items.Keys)
            {
                items.Add(itemCategory, _items[itemCategory].ToArray());
            }
            return items;
        }
        
        public Item[] GetAll()
        {
            List<Item> items = new List<Item>();
            foreach (var itemValue in _items.Values)
            {
                foreach (Item item in itemValue)
                    items.Add(item);
            }
            return items.ToArray();
        }
    
        private void Add(ItemDataSO itemData)
        {
            if (!_items.ContainsKey(itemData.Category))
            {
                _items.Add(itemData.Category, new HashSet<Item>());
            }
            
            Item item = new Item(itemData.Sprites, itemData.Category, itemData.SpriteCenterOffset, itemData.GetInstanceID(), itemData.AlignSizeByWidth, itemData.Unlocked);
            _items[item.Category].Add(item);
        }
    }
}