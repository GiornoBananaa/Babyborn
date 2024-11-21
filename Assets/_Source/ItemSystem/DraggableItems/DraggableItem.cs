using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ItemSystem.DraggableItems
{
    public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        [SerializeField] private float _scaleFactor = 1f;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        private Vector2 _normalScale;
        private Vector2 _offset;
        private Vector2 _defaultPosition;
        private Tween _animation;
        private bool _isAnimation;
        private bool _eventDataRegistered;
        private Camera _camera;

        public event Action<DraggableItem> OnReturn;
        public event Action<DraggableItem> OnDragEnd;
        
        public bool IsDragged { get; private set; }

        private void Awake()
        {
            _defaultPosition = transform.localPosition;
            _normalScale = transform.localScale;
            _camera = Camera.main;
        }
        
        public void Update()
        {
            if (!IsDragged || !enabled || _isAnimation) return;
            
            Vector2 mouseWorldPos = _camera!.ScreenToWorldPoint(Pointer.current.position.value);
            transform.position = mouseWorldPos + _offset;
        }

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!enabled || _isAnimation || IsDragged) return;
            _eventDataRegistered = true;
            StartDrag();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!(!_eventDataRegistered && IsDragged)) return;
            eventData.pointerPress = gameObject;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!enabled || _isAnimation || !IsDragged) return;
            EndDrag();
        }
        
        public void StartDrag()
        {
            if (_isAnimation)
            {
                _animation?.Kill();
            }
            IsDragged = true;
            transform.localScale = _normalScale * _scaleFactor;
            _offset = transform.position - _camera!.ScreenToWorldPoint(Pointer.current.position.value);
            OnDragStarted();
        }
        
        public void EndDrag()
        {
            IsDragged = false;
            transform.localScale = _normalScale;
            _eventDataRegistered = false;
            OnDragEnded();
            OnDragEnd?.Invoke(this);
        }
        
        public void ReturnToDefaultPosition()
        {
            if(_isAnimation) return;
            _isAnimation = true;
            if(IsDragged)
            {
                IsDragged = false;
                transform.localScale = _normalScale;
            }
            _animation = transform.DOLocalMove(_defaultPosition, 0.2f).OnComplete(() =>
            {
                _isAnimation = false;
                OnReturn?.Invoke(this);
            });
        }
        protected virtual void OnDragStarted(){}
        protected virtual void OnDragEnded(){}
    }
}