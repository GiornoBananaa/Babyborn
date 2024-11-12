using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ItemMenuSelector : MonoBehaviour
    {
        //TODO: ItemMenuView -> ICategoryObject
        [SerializeField] private ItemMenuView[] _itemMenus;
        [SerializeField] private ItemCategoryButton[] _categoryButtons;
        
        private readonly Dictionary<ItemCategory, List<ItemMenuView>> _itemMenusByCategories = new();
        
        [field: SerializeField] public ItemCategory SelectedCategory { get; private set; }
        
        private void Start()
        {
            SetItemMenusByCategories(_itemMenus);
            AddButtonListeners();
            foreach (var itemMenu in _itemMenus)
            {
                HideItemMenu(itemMenu);
            }
            SelectCategory(SelectedCategory);
        }

        public void SelectCategory(ItemCategory category)
        {
            if (SelectedCategory == category) return;
            
            foreach (var itemMenu in _itemMenusByCategories[SelectedCategory])
            {
                HideItemMenu(itemMenu);
            }
            
            foreach (var itemMenu in _itemMenusByCategories[category])
            {
                ShowItemMenu(itemMenu);
            }
            
            SelectedCategory = category;
        }

        private void ShowItemMenu(ItemMenuView itemMenu)
        {
            itemMenu.gameObject.SetActive(true);
        }
        
        private void HideItemMenu(ItemMenuView itemMenu)
        {
            itemMenu.gameObject.SetActive(false);
        }

        private void AddButtonListeners()
        {
            foreach (var categoryButtons in _categoryButtons)
            {
                categoryButtons.Button.onClick.AddListener(
                    () => SelectCategory(categoryButtons.Category));
            }
        }
        
        private void SetItemMenusByCategories(ItemMenuView[] itemMenus)
        {
            foreach (var itemMenu  in itemMenus)
            {
                if(!_itemMenusByCategories.ContainsKey(itemMenu.Category))
                    _itemMenusByCategories.Add(itemMenu.Category, new List<ItemMenuView>());
                _itemMenusByCategories[itemMenu.Category].Add(itemMenu);
            }
        }
    }
}
