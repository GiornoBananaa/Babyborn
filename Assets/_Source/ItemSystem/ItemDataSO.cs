using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Config/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [field: SerializeField] public Vector2 SpriteCenterOffset { get; private set; }
        [field: SerializeField] public bool AlignSizeByWidth { get; private set; }
        [field: SerializeField] public bool Unlocked { get; private set; }
        [field: SerializeField] public bool SaveSelection { get; private set; } = true;
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
        [field: SerializeField] public ItemProperty[] Properties { get; private set; } = Array.Empty<ItemProperty>();
        

        [Serializable]
        public class ItemProperty
        {
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public float Value { get; private set; }
        }
    }
}