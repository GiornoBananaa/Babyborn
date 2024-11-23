using System;
using R3;
using UnityEngine;

namespace ItemSystem
{
    public class Item
    {
        public readonly Sprite Sprite;
        public readonly ItemCategory Category;
        public readonly Vector2 CenterOffset;
        public readonly bool AlignSizeByWidth;
        public readonly int ID;
        public readonly ReactiveProperty<bool> Unlocked = new(false);
        public readonly ReactiveProperty<bool> Selected = new(false);

        public Item(Sprite sprite, ItemCategory category, Vector2 centerOffset, int id, bool alignSizeByWidth, bool unlocked)
        {
            Sprite = sprite;
            Category = category;
            CenterOffset = centerOffset;
            AlignSizeByWidth = alignSizeByWidth;
            Unlocked.Value = unlocked;
            ID = id;
        }
    
        public void UnlockItem()
        {
            Unlocked.Value = true;
        }
    }
}