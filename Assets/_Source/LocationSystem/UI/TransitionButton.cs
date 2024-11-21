using UnityEngine;
using UnityEngine.UI;

namespace LocationSystem.UI
{
    [RequireComponent(typeof(Button))]
    public class TransitionButton : MonoBehaviour
    {
        [field: SerializeField] public LocationType Location { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
    }
}