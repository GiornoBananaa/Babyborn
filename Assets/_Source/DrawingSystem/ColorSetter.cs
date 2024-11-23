using FreeDraw;
using ItemSystem;
using UnityEngine;
using VContainer;

public class ColorSetter : MonoBehaviour
{
        [SerializeField] public ItemCategory _category;
        [Space(5)]
        [SerializeField] private DrawingSettings _drawingSettings;
        
        private Item _item;
        private ItemSelector _itemSelector;
        
        [Inject]
        public void Construct(ItemSelector itemSelector)
        {
            _itemSelector = itemSelector;
            _itemSelector.OnItemSelected += SetColor;
        }
        
        private void SetColor(Item item)
        {
            if(item.Category != _category) return;
            
            //_drawingSettings.SetMarkerColour(item.Sprite.texture.GetPixel(0,0));
            _drawingSettings.SetTexture(item.Sprite);
            _item = item;
        }
        
        private void OnDestroy()
        {
            _itemSelector.OnItemSelected -= SetColor;
        }
}
