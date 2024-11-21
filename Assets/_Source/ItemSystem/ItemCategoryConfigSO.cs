using System;
using ItemSystem;
using UnityEngine;

namespace ClothesSystem
{
    [CreateAssetMenu(fileName = "ItemCategoryConfig", menuName = "Config/ItemCategoryConfig")]
    public class ItemCategoryConfigSO : ScriptableObject
    {
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [field: SerializeField] public int MaxSelectedCount { get; private set; }
        [field: SerializeField] public bool UnselectOldSelection { get; private set; } = true;
        [field: SerializeField] public bool SelectByPointerUp { get; private set; } = true;
        [field: SerializeField] public ItemCategory[] OverlappedCategories { get; private set; } = Array.Empty<ItemCategory>();
    }
}