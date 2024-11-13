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
        
        private ItemSelector _itemSelector;
        
        [Inject]
        public void Construct(ItemSelector itemSelector)
        {
            _itemSelector = itemSelector;
            _itemSelector.OnItemSelected += SetSprite;
        }

        private void SetSprite(Item item)
        {
            if(item.Category != _category) return;
            
            if (_spriteRenderer != null)
                _spriteRenderer.sprite = item.Sprite;
            else
                _image.sprite = item.Sprite;
        }
        
        private void OnDestroy()
        {
            _itemSelector.OnItemSelected -= SetSprite;
        }
    }
}
