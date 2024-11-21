using System;
using UnityEngine;

namespace ItemSystem.DraggableItems
{
    public class PlaceableItem : DraggableItem
    {
        [SerializeField] private LayerMask _surfaceLayerMask;
        private Vector2[] _raycastPoints;
        
        protected override void OnDragStarted()
        {
            transform.parent = null;
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
        
        protected override void OnDragEnded()
        {
            CheckSurface();
        }

        private Vector2[] GetRaycastPoints()
        {
            var raycastPoints = new Vector2[5];
            var spriteBounds = _spriteRenderer.sprite.bounds;
            var spriteExtents = _spriteRenderer.sprite.bounds.extents;
            raycastPoints[0] = (Vector2)_spriteRenderer.transform.position + new Vector2(spriteBounds.center.x * spriteExtents.x, spriteBounds.center.y * spriteExtents.y);
            raycastPoints[1] = (Vector2)_spriteRenderer.transform.position + new Vector2(spriteBounds.min.x * spriteExtents.x, spriteBounds.min.y * spriteExtents.y);
            raycastPoints[2] = (Vector2)_spriteRenderer.transform.position + new Vector2(spriteBounds.max.x * spriteExtents.x, spriteBounds.max.y * spriteExtents.y);
            raycastPoints[3] = (Vector2)_spriteRenderer.transform.position + new Vector2(new Vector2(spriteBounds.min.x, spriteBounds.max.y).x * spriteExtents.x, new Vector2(spriteBounds.min.x, spriteBounds.max.y).y * spriteExtents.y); 
            raycastPoints[4] = (Vector2)_spriteRenderer.transform.position + new Vector2(new Vector2(spriteBounds.max.x, spriteBounds.min.y).x * spriteExtents.x, new Vector2(spriteBounds.max.x, spriteBounds.min.y).y * spriteExtents.y);
            return raycastPoints;
        }
        
        
        private void OnDrawGizmos()
        {
            if(_spriteRenderer.sprite == null) return;
            var raycastPoints = new Vector2[5];
            var spriteBounds = _spriteRenderer.sprite.bounds;
            var spriteExtents = _spriteRenderer.sprite.bounds.extents;
            raycastPoints[0] = (Vector2)_spriteRenderer.transform.position + new Vector2(spriteBounds.center.x * spriteExtents.x, spriteBounds.center.y * spriteExtents.y);
            raycastPoints[1] = (Vector2)_spriteRenderer.transform.position + new Vector2(spriteBounds.min.x * spriteExtents.x, spriteBounds.min.y * spriteExtents.y);
            raycastPoints[2] = (Vector2)_spriteRenderer.transform.position + new Vector2(spriteBounds.max.x * spriteExtents.x, spriteBounds.max.y * spriteExtents.y);
            raycastPoints[3] = (Vector2)_spriteRenderer.transform.position + new Vector2(new Vector2(spriteBounds.min.x, spriteBounds.max.y).x * spriteExtents.x, new Vector2(spriteBounds.min.x, spriteBounds.max.y).y * spriteExtents.y); 
            raycastPoints[4] = (Vector2)_spriteRenderer.transform.position + new Vector2(new Vector2(spriteBounds.max.x, spriteBounds.min.y).x * spriteExtents.x, new Vector2(spriteBounds.max.x, spriteBounds.min.y).y * spriteExtents.y);

            foreach (var VARIABLE in raycastPoints)
            {
                Gizmos.DrawSphere(VARIABLE, 0.1f);
            }
            
        }

        private void CheckSurface()
        {
            Collider2D surface = null;
            foreach (var point in GetRaycastPoints())
            {
                surface = Physics2D.OverlapPoint(point, _surfaceLayerMask);
                if(surface != null)
                    break;
            }
            
            if (surface != null)
            {
                _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                transform.SetParent(surface.transform);
            }
            else
            {
                ReturnToDefaultPosition();
            }
        }
    }
}
