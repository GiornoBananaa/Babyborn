using System;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ItemSystem.UI
{
    public class MenuItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _lock;
        [SerializeField] private RectTransform _selection;
        [SerializeField] private bool _pointerUpForSelect = true;
        
        private int _cellSize;
        private Item _item;
        private CompositeDisposable _itemSubscription = new();

        public UnityEvent OnSelected;
        
        public void Construct(Item item, int cellSize, bool pointerUpForSelect)
        {
            _cellSize = cellSize;
            _pointerUpForSelect = pointerUpForSelect;
            _item = item;
            _image.sprite = _item.Sprite;
            var rectTransform = _image.rectTransform;
            rectTransform.pivot = new Vector2(0.5f, 0.5f) - _item.CenterOffset;
            float spriteRatio = _item.Sprite.bounds.size.x / _item.Sprite.bounds.size.y;
            rectTransform.sizeDelta = new Vector2(spriteRatio * rectTransform.sizeDelta.y, rectTransform.sizeDelta.y);
            if (item.AlignSizeByWidth)
            {
                rectTransform.sizeDelta = new Vector2(_cellSize, _cellSize/spriteRatio);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(_cellSize * spriteRatio, _cellSize);
            }
            ShowLock(!_item.Unlocked.Value);
            _itemSubscription.Add(_item.Unlocked.Subscribe(ShowLock));
            _itemSubscription.Add(item.Selected.Subscribe(ShowSelection));
            
            if(_pointerUpForSelect)
                _button.onClick.AddListener(() => OnSelected?.Invoke());
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
            if(!_pointerUpForSelect)
                OnSelected?.Invoke();
        }
    }
}