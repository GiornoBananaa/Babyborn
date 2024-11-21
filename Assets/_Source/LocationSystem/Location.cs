using UnityEngine;

namespace LocationSystem
{
    public class Location : MonoBehaviour
    {
        [field: SerializeField] public LocationType Type { get; private set; }
        [field: SerializeField] public Transform CameraPosition { get; private set; }
        [field: SerializeField] public RectTransform MainPanel { get; private set; }
    }
}