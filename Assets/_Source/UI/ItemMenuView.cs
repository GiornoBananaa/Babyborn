using UnityEngine;

namespace UI
{
    public class ItemMenuView : MonoBehaviour
    {
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [SerializeField] private RectTransform _panel;
        [SerializeField] private RectTransform _itemListParent;
    }
}