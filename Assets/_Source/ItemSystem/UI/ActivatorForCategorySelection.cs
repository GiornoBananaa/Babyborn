using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ItemSystem.UI
{
    public class ActivatorForCategorySelection : MonoBehaviour
    {
        [Serializable]
        public class ActivationObjectsByCategory
        {
            public ItemCategory ItemCategory;
            public Behaviour[] Behaviours;
            public GameObject[] Objects;
        }
        
        [SerializeField] public CategoryMenuSelector _menuSelector;
        [SerializeField] public ActivationObjectsByCategory[] _sceneItems;
        
        private Dictionary<ItemCategory, ActivationObjectsByCategory> _activations;
        private Item _item;
        
        public void Awake()
        {
            _menuSelector.OnCategorySelected += OnSelect;
            _menuSelector.OnCategoryUnselected += OnUnselect;
            _activations = _sceneItems.ToDictionary(
                activation => activation.ItemCategory, 
                activation => activation);
        }

        private void OnSelect(ItemCategory item)
        {
            if(!_activations.TryGetValue(item, out var activations)) return;
            
            foreach (var behaviour in activations.Behaviours)
            {
                behaviour.enabled = true;
            }
            foreach (var obj in activations.Objects)
            {
                obj.SetActive(true);
            }
        }
        
        private void OnUnselect(ItemCategory item)
        {
            if(!_activations.TryGetValue(item, out var activations)) return;
            
            foreach (var behaviour in activations.Behaviours)
            {
                behaviour.enabled = false;
            }
            foreach (var obj in activations.Objects)
            {
                obj.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            _menuSelector.OnCategorySelected -= OnSelect;
            _menuSelector.OnCategoryUnselected -= OnUnselect;
        }
    }
}