using UnityEngine;
using UnityEngine.UI;

namespace ItemSystem.UI
{
    [RequireComponent(typeof(Button))]
    public class ItemCategoryButton: MonoBehaviour
    {
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
    }
}