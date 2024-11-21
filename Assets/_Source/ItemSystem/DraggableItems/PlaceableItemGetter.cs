using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace ItemSystem.DraggableItems
{
    public class PlaceableItemGetter : MonoBehaviour
    {
        [SerializeField] public ItemCategory _category;
        [Space(5)]
        [SerializeField] private PlaceableItem _placeableItemPrefab;
        
        private PlaceableItem _currentPlaceable;
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

        private void InstantiateItem(Item item)
        {
            if(item.Category != _category) return;
            
            _currentPlaceable = Instantiate(_placeableItemPrefab, _camera.ScreenToWorldPoint(Pointer.current.position.value), Quaternion.identity);
            _currentItem = item;
            _currentPlaceable.SetSprite(item.Sprite);
            _currentPlaceable.StartDrag();
            _currentPlaceable.OnDragEnd += UnselectPlaceable;
            _currentPlaceable.OnReturn += ReturnPlaceable;
        }

        private void UnselectPlaceable(DraggableItem item)
        {
            _itemSelector.Unselect(_currentItem);
        }

        private void ReturnPlaceable(DraggableItem item)
        {
            Destroy(_currentPlaceable.gameObject);
        }


        private void OnDestroy()
        {
            if (_currentPlaceable != null)
            {
                _currentPlaceable.OnDragEnd -= UnselectPlaceable;
                _currentPlaceable.OnReturn -= ReturnPlaceable;
            }
            if(_itemSelector != null)
                _itemSelector.OnItemSelected -= InstantiateItem;
        }
    }
}
