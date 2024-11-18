using ItemSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace ClothesSystem
{
    public class SelectedItemSpriteSetter : MonoBehaviour
    {
        [SerializeField] public ItemCategory _category;
        [Space(5)]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Header("Alternative")]
        [SerializeField] private Image _image;
        
        private Item _item;
        private ItemSelector _itemSelector;
        
        [Inject]
        public void Construct(ItemSelector itemSelector)
        {
            _itemSelector = itemSelector;
            _itemSelector.OnItemSelected += SetSprite;
            _itemSelector.OnItemUnselected += SetTransparent;
        }

        private void SetSprite(Item item)
        {
            if(item.Category != _category) return;
            
            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;
                _spriteRenderer.sprite = item.Sprite;
            }
            else
            {
                _image.enabled = true;
                _image.sprite = item.Sprite;
            }
            
            _item = item;
        }
        
        private void SetTransparent(Item item)
        {
            if(_item != item) return;
            
            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = false;
            }
            else
            {
                _spriteRenderer.enabled = false;
            }
        }
        
        private void OnDestroy()
        {
            _itemSelector.OnItemSelected -= SetSprite;
        }
    }
}
