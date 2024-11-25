using System;
using System.Collections.Generic;
using NUnit.Framework;
using StickersSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VContainer;

namespace ItemSystem.DraggableItems
{
    public class PlaceableItemGetter : MonoBehaviour
    {
        [SerializeField] public ItemCategory _category;
        [Space(5)]
        [SerializeField] private Sticker _stickerPrefab;
        [SerializeField] private StickerCopier _stickerCopier; //TODO eh
        
        private Sticker _currentSticker;
        private List<Sticker> _stickers = new();
        private Item _currentItem;
        private ItemSelector _itemSelector;
        private Camera _camera;
        
        [Inject]
        public void Construct(ItemSelector itemSelector)
        {
            _itemSelector = itemSelector;
            _itemSelector.OnItemSelected += InstantiateItem;
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            foreach (var items in _stickers)
            {
                items.enabled = true;
            }
        }
        
        private void OnDisable()
        {
            foreach (var items in _stickers)
            {
                items.enabled = false;
            }
        }

        private void InstantiateItem(Item item)
        {
            if(item.Category != _category) return;
            _currentSticker = Instantiate(_stickerPrefab, _camera.ScreenToWorldPoint(Pointer.current.position.value), Quaternion.identity);
            _currentSticker.SetItem(item);
            _stickers.Add(_currentSticker);
            _currentItem = item;
            _currentSticker.SetSprite(item.Sprite);
            _currentSticker.StartDrag();
            _currentSticker.OnDragEnd += OnPlaced;
            _currentSticker.OnReturn += ReturnSticker;
            
        }

        private void OnPlaced(DraggableItem item)
        {
            _stickerCopier.AddSticker((Sticker)item);
            UnselectSticker(item);
        }
        
        private void UnselectSticker(DraggableItem item)
        {
            _itemSelector.Unselect(_currentItem);
        }

        private void ReturnSticker(DraggableItem item)
        {
            item.OnDragEnd -= UnselectSticker;
            item.OnReturn -= ReturnSticker;
            _stickers.Remove((Sticker)item);
            _stickerCopier.RemoveSticker(_currentSticker);
            Destroy(_currentSticker.gameObject);
        }


        private void OnDestroy()
        {
            if (_currentSticker != null)
            {
                _currentSticker.OnDragEnd -= UnselectSticker;
                _currentSticker.OnReturn -= ReturnSticker;
            }
            if(_itemSelector != null)
                _itemSelector.OnItemSelected -= InstantiateItem;
        }
    }
}
