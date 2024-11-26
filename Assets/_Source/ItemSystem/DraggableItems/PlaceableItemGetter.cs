using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private Dictionary<int, Item> _itemsSticker;
        private List<Sticker> _stickers = new();
        private ItemSelector _itemSelector;
        private Camera _camera;
        
        [Inject]
        public void Construct(ItemSelector itemSelector, ItemContainer itemContainer)
        {
            _itemSelector = itemSelector;
            _itemsSticker = itemContainer.Get(_category).ToDictionary(item => item.ID);
            _itemSelector.OnItemSelected += InstantiateItem;
            _camera = Camera.main;
        }

        private void Start()
        {
            LoadStickers();
        }
        
        private void LoadStickers()
        {/*
            foreach ()
            {
                InstantiateItem();
                var sticker = Instantiate(_stickerPrefab, _camera.ScreenToWorldPoint(Pointer.current.position.value), Quaternion.identity);
                sticker.SetItem(item);
                _stickers.Add(sticker);
                sticker.SetSprite(item.Sprite);
                sticker.StartDrag();
                sticker.OnDragEnd += OnPlaced;
                sticker.OnReturn += ReturnSticker;
            }*/
        }
        
        private void SaveStickers()
        {
            
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

            SaveStickers();
        }

        private void InstantiateItem(Item item)
        {
            if(item.Category != _category) return;
            var sticker = Instantiate(_stickerPrefab, _camera.ScreenToWorldPoint(Pointer.current.position.value), Quaternion.identity);
            sticker.SetItem(item);
            _stickers.Add(sticker);
            sticker.SetSprite(item.Sprite);
            sticker.StartDrag();
            sticker.OnDragEnd += OnPlaced;
            sticker.OnReturn += ReturnSticker;
            
        }

        private void OnPlaced(DraggableItem item)
        {
            _stickerCopier.AddSticker((Sticker)item);
            UnselectSticker(item);
        }
        
        private void UnselectSticker(DraggableItem item)
        {
            _itemSelector.Unselect(((Sticker)item).Item);
        }

        private void ReturnSticker(DraggableItem item)
        {
            item.OnDragEnd -= UnselectSticker;
            item.OnReturn -= ReturnSticker;
            _stickers.Remove((Sticker)item);
            _stickerCopier.RemoveSticker((Sticker)item);
            Destroy(item.gameObject);
        }


        private void OnDestroy()
        {
            foreach (var sticker in _stickers)
            {
                if(sticker == null) continue;
                sticker.OnDragEnd -= UnselectSticker;
                sticker.OnReturn -= ReturnSticker;
            }
            if(_itemSelector != null)
                _itemSelector.OnItemSelected -= InstantiateItem;
        }
    }
}
