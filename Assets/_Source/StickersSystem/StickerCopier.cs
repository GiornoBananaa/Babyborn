using System.Collections.Generic;
using ItemSystem.DraggableItems;
using UnityEngine;

namespace StickersSystem
{
    public class StickerCopier : MonoBehaviour
    {
        [SerializeField] private Transform[] _stickerParents;
        
        private Dictionary<int, List<Sticker>> _copiedStickers = new();
        
        public void AddSticker(Sticker sticker)
        {
            int id = sticker.GetInstanceID();
            if(!_copiedStickers.ContainsKey(id))
            {
                foreach (var parent in _stickerParents)
                {
                    CopyStick(parent, sticker);
                }
            }
            UpdateSticker(sticker);
        }
        
        public void RemoveSticker(Sticker sticker)
        {
            int id = sticker.GetInstanceID();
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
