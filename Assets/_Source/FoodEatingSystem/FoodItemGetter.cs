using System.Collections.Generic;
using ItemSystem;
using ItemSystem.DraggableItems;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace FoodEatingSystem
{
    public class FoodItemGetter : MonoBehaviour
    {
        [Space(5)]
        [SerializeField] private Food _foodPrefab;
        
        private readonly List<Food> _stickers = new List<Food>();
        private ItemSelector _itemSelector;
        private Camera _camera;
        
        [Inject]
        public void Construct(ItemSelector itemSelector)
        {
            _itemSelector = itemSelector;
            _itemSelector.OnItemSelected += InstantiateItem;
            _camera = Camera.main;
        }

        private void InstantiateItem(Item item)
        {
            if(item.Category != ItemCategory.Food) return;
            var sticker = Instantiate(_foodPrefab, _camera.ScreenToWorldPoint(Pointer.current.position.value), Quaternion.identity);
            sticker.SetItem(item);
            _stickers.Add(sticker);
            sticker.SetSprite(item.Sprite);
            sticker.StartDrag();
            sticker.OnDragEnd += OnDragEnd;
            sticker.OnReturn += ReturnSticker;
            
        }
        
        private void OnDragEnd(DraggableItem item)
        {
            UnselectSticker(item);
        }
        
        private void UnselectSticker(DraggableItem item)
        {
            _itemSelector.Unselect(((Food)item).Item);
        }

        private void ReturnSticker(DraggableItem item)
        {
            item.OnDragEnd -= UnselectSticker;
            item.OnReturn -= ReturnSticker;
            _stickers.Remove((Food)item);
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