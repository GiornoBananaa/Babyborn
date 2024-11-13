using R3;
using UnityEngine;

namespace ItemSystem
{
    public class Item
    {
        public readonly Sprite Sprite;
        public readonly ItemCategory Category;
        public readonly ReactiveProperty<bool> Unlocked = new(false);
        public readonly ReactiveProperty<bool> Selected = new(false);

        public Item(Sprite sprite, ItemCategory category, bool unlocked)
        {
            Sprite = sprite;
            Category = category;
            Unlocked.Value = unlocked;
        }
    
        public void UnlockItem()
        {
            Unlocked.Value = true;
        }
    }
}