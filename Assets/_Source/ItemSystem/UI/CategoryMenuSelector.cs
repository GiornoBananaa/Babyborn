using System;
using System.Collections.Generic;
using System.Linq;
using FreeDraw;
using UnityEngine;
using VContainer;

namespace ItemSystem.UI
{
    public class CategoryMenuSelector : MonoBehaviour
    {
        [SerializeField] private bool _closeOnSecondClick = false;
        [SerializeField] private ItemCategory[] _selectableCategories;
        
        private ItemMenuView[] _itemMenus;
        private ItemCategoryButton[] _categoryButtons;
        private HashSet<ItemCategory> _categories;
        
        private readonly Dictionary<ItemCategory, List<ItemMenuView>> _itemMenusByCategories = new();
        [field: SerializeField] public ItemCategory SelectedCategory { get; private set; }
        
        public event Action<ItemCategory> OnCategorySelected;
        public event Action<ItemCategory> OnCategoryUnselected;
        
        [Inject]
        public void Construct(IEnumerable<ItemMenuView> itemMenus, IEnumerable<ItemCategoryButton> categoryButtons)
        {
            _categories = new HashSet<ItemCategory>(_selectableCategories);
            _itemMenus = itemMenus.Where(menu => _categories.Contains(menu.Category)).ToArray();
            _categoryButtons = categoryButtons.Where(button => _categories.Contains(button.Category)).ToArray();
        }
        
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

        private void OnDestroy()
        {
            RemoveButtonListeners();
        }

        public void SelectCategory(ItemCategory category)
        {
            if(SelectedCategory != default)
            {
                foreach (var itemMenu in _itemMenusByCategories[SelectedCategory])
                {
                    HideItemMenu(itemMenu);
                }
            }
            
            foreach (var itemMenu in _itemMenusByCategories[category])
            {
                ShowItemMenu(itemMenu);
            }
            
            SelectedCategory = category;
            OnCategorySelected?.Invoke(SelectedCategory);
        }
        
        public void UnselectCategory()
        {
            foreach (var itemMenu in _itemMenusByCategories[SelectedCategory])
            {
                HideItemMenu(itemMenu);
            }
            
            ItemCategory unselectedCategory = SelectedCategory;
            SelectedCategory = default;
        }
        
        private void OnCategoryClicked(ItemCategory category)
        {
            if (SelectedCategory == category && _closeOnSecondClick)
            {
                UnselectCategory();
            }
            else
            {
                SelectCategory(category);
            }
        }
        
        private void ShowItemMenu(ItemMenuView itemMenu)
        {
            itemMenu.gameObject.SetActive(true);
        }
        
        private void HideItemMenu(ItemMenuView itemMenu)
        {
            itemMenu.gameObject.SetActive(false);
            OnCategoryUnselected?.Invoke(itemMenu.Category);
        }

        private void AddButtonListeners()
        {
            foreach (var categoryButtons in _categoryButtons)
            {
                categoryButtons.Button.onClick.AddListener(
                    () => OnCategoryClicked(categoryButtons.Category));
            }
        }
        
        private void RemoveButtonListeners()
        {
            foreach (var categoryButtons in _categoryButtons)
            {
                categoryButtons.Button.onClick.RemoveListener(
                    () => OnCategoryClicked(categoryButtons.Category));
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
