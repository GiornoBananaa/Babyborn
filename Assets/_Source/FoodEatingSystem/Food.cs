using ItemSystem;
using ItemSystem.DraggableItems;
using UnityEngine;

namespace FoodEatingSystem
{
    public class Food : DraggableItem
    {
        private const string _satietyPropertyName = "Satiety";
        
        [SerializeField] private LayerMask _surfaceLayerMask;
        
        private Item _item;
        
        public Item Item => _item;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        public void SetItem(Item item)
        {
            _item = item;
        }
        
        protected override void OnDragStarted()
        {
            transform.parent = null;
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
        
        protected override void OnDragEnded()
        {
            CheckSurface();
        }
        
        private void CheckSurface()
        {
            var surface = Physics2D.OverlapPoint(_spriteRenderer.transform.position, _surfaceLayerMask);
            
            if(surface != null && surface.TryGetComponent<StatusController>(out var eater))
            {
                if(_item.TryGetFloat(_satietyPropertyName, out float satiety))
                {
                    eater.EatFood(satiety);
                    ReturnItem();
                    return;
                }
            }
            
            ReturnToDefaultPosition();
        }
    }
}