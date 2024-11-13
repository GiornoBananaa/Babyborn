using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Config/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [field: SerializeField] public bool Unlocked { get; private set; }
    }
}