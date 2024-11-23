using System;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace ItemSystem.UI
{
    public class SceneItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _lock;
        [SerializeField] private RectTransform _selection;
        [SerializeField] private bool _pointerUpForSelect = true;
        [SerializeField] private ItemDataSO _itemData;
        
        private Item _item;
        private readonly CompositeDisposable _itemSubscription = new();
        
        public event Action<Item> OnClick;
        
        [Inject]
        public void Construct(ItemContainer itemContainer)
        {
            SetItem(itemContainer.Get(_itemData.Category, _itemData.GetInstanceID()));
        }
        
        private void SetItem(Item item)
        {
            _item = item;
            _image.sprite = _item.Sprite;
            var rectTransform = _image.rectTransform;
            rectTransform.pivot = new Vector2(0.5f, 0.5f) - _item.CenterOffset;
            float spriteRatio = _item.Sprite.bounds.size.x / _item.Sprite.bounds.size.y;
            rectTransform.sizeDelta = new Vector2(spriteRatio * rectTransform.sizeDelta.y, rectTransform.sizeDelta.y);
            ShowLock(!_item.Unlocked.Value);
            _itemSubscription.Add(_item.Unlocked.Subscribe(ShowLock));
            _itemSubscription.Add(item.Selected.Subscribe(ShowSelection));
            
            if(_pointerUpForSelect)
                _button.onClick.AddListener(() => OnClick?.Invoke(_item));
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
            _button.interactable = !isLocked;
            _lock.gameObject.SetActive(isLocked);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_pointerUpForSelect)
                OnClick?.Invoke(_item);
        }
    }
}