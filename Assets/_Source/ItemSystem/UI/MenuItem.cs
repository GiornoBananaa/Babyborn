using System;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ItemSystem.UI
{
    public class MenuItem : MonoBehaviour
    {
        [field: SerializeField] public Button UseButton { get; private set; }
        [SerializeField] private Image _image;
        [SerializeField] public RectTransform _lock;
        [SerializeField] public RectTransform _selection;
        
        private Item _item;
        private CompositeDisposable _itemSubscription = new();
        
        public void Construct(Item item)
        {
            _item = item;
            _image.sprite = _item.Sprite;
            ShowLock(!_item.Unlocked.Value);
            _itemSubscription.Add(_item.Unlocked.Subscribe(ShowLock));
            _itemSubscription.Add(item.Selected.Subscribe(ShowSelection));
        }

        private void OnDestroy()
        {
            _itemSubscription?.Dispose();
        }
        
        private void ShowSelection(bool isSelected)
        {
            _selection.gameObject.SetActive(isSelected);
        }
        
        private void ShowLock(bool isLocked)
        {
            UseButton.interactable = !isLocked;
            _lock.gameObject.SetActive(isLocked);
        }
    }
}