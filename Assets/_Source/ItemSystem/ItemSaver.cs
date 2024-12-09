using System;
using System.IO;
using UnityEngine;

namespace ItemSystem
{
    public class ItemSaver
    {
        [Serializable]
        private class ItemData
        {
            public bool Selected;
            public bool Unlocked;
        }
        
        private readonly string _globalPath = Application.persistentDataPath + "/" + "{0}.json";
        private readonly ItemContainer _itemContainer;
        private readonly ItemSelector _itemSelector;
        
        public ItemSaver(ItemContainer itemContainer, ItemSelector itemSelector)
        {
            _itemContainer = itemContainer;
            _itemSelector = itemSelector;
            _itemSelector.OnItemSelected += SaveItem;
            _itemSelector.OnItemUnselected += SaveItem;
        }
        
        public void LoadAllItems()
        {
            Debug.Log("LoadAllItems");
            foreach (var item in _itemContainer.GetAll())
            {
                LoadItem(item);
            }
        }

        public void DeleteAllData()
        {
            foreach (var item in _itemContainer.GetAll())
            {
                string path = string.Format(_globalPath, item.ID);
                if(File.Exists(path))
                    File.Delete(path);
            }
        }
        
        private void SaveItem(Item item)
        {
            string path = string.Format(_globalPath, item.ID);
            Debug.Log("Saving item to " + path);
            string json = JsonUtility.ToJson(new ItemData
            {
                Selected = item.Selected.Value, 
                Unlocked = item.Unlocked.Value
            });
            File.WriteAllText(path, json);
        }

        private void LoadItem(Item item)
        {
            string path = string.Format(_globalPath, item.ID);
            if(File.Exists(path))
            {
                Debug.Log("Saving item to " + path);
                string json = File.ReadAllText(path);
                ItemData data = JsonUtility.FromJson<ItemData>(json);
                
                item.Unlocked.Value = data.Unlocked;
                
                if(data.Selected)
                    _itemSelector.Select(item);
                else
                    _itemSelector.Unselect(item);
            }
        }
    }
}