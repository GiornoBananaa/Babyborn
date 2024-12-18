using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace ItemSystem
{
    public class Item
    {
        public readonly Sprite[] Sprites;
        public Sprite Sprite => Sprites[0];

        public readonly ItemCategory Category;
        public readonly Vector2 CenterOffset;
        public readonly bool AlignSizeByWidth;
        public readonly int ID;
        public readonly ReactiveProperty<bool> Unlocked = new(false);
        public readonly ReactiveProperty<bool> Selected = new(false);
        public readonly bool SaveSelection;
        
        private readonly Dictionary<string, float> _properties;

        public Item(Sprite[] sprites, ItemCategory category, Vector2 centerOffset, int id, 
            bool alignSizeByWidth, bool unlocked, Dictionary<string, float> properties, bool saveSelection)
        {
            Sprites = sprites;
            Category = category;
            CenterOffset = centerOffset;
            AlignSizeByWidth = alignSizeByWidth;
            _properties = properties;
            Unlocked.Value = unlocked;
            SaveSelection = saveSelection;
            ID = id;
        }
    
        public void UnlockItem()
        {
            Unlocked.Value = true;
        }

        public bool TryGetFloat(string satietyPropertyName, out float value)
        {
            return _properties.TryGetValue(satietyPropertyName, out value);
        }
    }
}