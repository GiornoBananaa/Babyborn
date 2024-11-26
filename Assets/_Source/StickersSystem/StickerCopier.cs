using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ItemSystem.DraggableItems;
using UnityEngine;

namespace StickersSystem
{
    public class StickerCopier : MonoBehaviour
    {
        [SerializeField] private Transform[] _stickerParents;
        
        private Dictionary<int, List<Sticker>> _copiedStickers = new();
        private Dictionary<int, Sticker> _originalStickers = new();
        
        public void AddSticker(Sticker sticker)
        {
            int id = sticker.GetInstanceID();
            if(!_copiedStickers.ContainsKey(id))
            {
                _originalStickers.Add(id, sticker);
                foreach (var parent in _stickerParents)
                {
                    CopyStick(parent, sticker);
                }

                UpdateRenderers();
            }
            UpdateSticker(sticker);
        }

        private async UniTaskVoid UpdateRenderers()
        {
            await UniTask.DelayFrame(2);
            foreach (var originalSticker in _originalStickers.Values)
            {
                originalSticker.SpriteRenderer.sortingOrder =
                    originalSticker.SpriteRenderer.sortingOrder == 5 ? 6 : 5;
                originalSticker.SpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                originalSticker.SpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }
        
        public void RemoveSticker(Sticker sticker)
        {
            int id = sticker.GetInstanceID();
            _originalStickers.Remove(id);
            foreach (var copy in _copiedStickers[id])
            {
                Destroy(copy);
            }
            _copiedStickers.Remove(sticker.GetInstanceID());
        }

        private void UpdateSticker(Sticker sticker)
        {
            int id = sticker.GetInstanceID();
            foreach (var copy in _copiedStickers[id])
            {
                copy.transform.localScale = sticker.transform.localScale;
                copy.transform.localPosition = sticker.transform.localPosition;
                copy.transform.localRotation = sticker.transform.localRotation;
                copy.SpriteRenderer.sortingOrder = copy.SpriteRenderer.sortingOrder == 5 ? 6 : 5;
            }
        }
        
        private void CopyStick(Transform parent, Sticker sticker)
        {
            int id = sticker.GetInstanceID();
            Sticker copy = Instantiate(sticker, parent);
            if (!_copiedStickers.ContainsKey(id))
                _copiedStickers.Add(id, new List<Sticker>());
            _copiedStickers[id].Add(copy);
        }
    }
}
