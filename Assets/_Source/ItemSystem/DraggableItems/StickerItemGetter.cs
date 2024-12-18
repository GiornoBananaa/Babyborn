using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using StickersSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace ItemSystem.DraggableItems
{
    public class StickerItemGetter : MonoBehaviour
    {
        [Serializable]
        public class StickerData
        {
            public float X;
            public float Y;
            public float SizeX;
            public float SizeY;
            public int ItemID;

            public StickerData( int itemID, float x, float y, float sizeX, float sizeY)
            {
                ItemID = itemID;
                SizeX = sizeX;
                SizeY = sizeY;
                X = x;
                Y = y;
            }
        }
        
        private const string STICKER_SAVE = "Stickers";
        private const string STICKER_COUNT_SAVE = "StickersCount";
        
        [SerializeField] public ItemCategory _category;
        [Space(5)]
        [SerializeField] private Sticker _stickerPrefab;
        [SerializeField] private StickerCopier _stickerCopier;
        [SerializeField] private Transform _parent;
        
        private Dictionary<int, Item> _itemsSticker;
        private List<Sticker> _stickers = new();
        private ItemSelector _itemSelector;
        private Camera _camera;
        
        [Inject]
        public void Construct(ItemSelector itemSelector, ItemContainer itemContainer)
        {
            _itemSelector = itemSelector;
            _itemsSticker = itemContainer.Get(_category).ToDictionary(item => item.ID);
            _itemSelector.OnItemSelected += InstantiateItem;
            _camera = Camera.main;
        }

        private void Start()
        {
            LoadStickers();
        }

        private void DeleteData()
        {
            string path = Application.persistentDataPath + "/" + STICKER_SAVE;
            string countPath = Application.persistentDataPath + "/" + STICKER_COUNT_SAVE;
            if(!File.Exists(countPath)) return;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream countFileStream = new FileStream(countPath, FileMode.Open);
            int count = (int)formatter.Deserialize(countFileStream);
            countFileStream.Close();
            for (int i = 0; i < count; i++)
            {
                if(!File.Exists(path+i)) return;
                File.Delete(path+i);
            }
            File.Delete(countPath);
        }
        
        private void SaveStickers()
        {
            DeleteData();
            
            BinaryFormatter formatter = new BinaryFormatter();
            
            string path = Application.persistentDataPath + "/" + STICKER_SAVE;
            string countPath = Application.persistentDataPath + "/" + STICKER_COUNT_SAVE;
            int index = 0;
            
            foreach (var sticker in _stickers)
            {
                if(File.Exists(path+index))
                    File.Delete(path+index);
                FileStream stream = new FileStream(path+index, FileMode.Create);
                StickerData data = new StickerData(sticker.Item.ID, sticker.transform.localPosition.x, sticker.transform.localPosition.y,
                    sticker.transform.localScale.x, sticker.transform.localScale.y);
                formatter.Serialize(stream, data);
                stream.Close();
                index++;
            }
            
            
            FileStream countFileStream = new FileStream(countPath, FileMode.Create);
            formatter.Serialize(countFileStream, index);
            countFileStream.Close();
        }
        
        private void LoadStickers()
        {
            string path = Application.persistentDataPath + "/" + STICKER_SAVE;
            string countPath = Application.persistentDataPath + "/" + STICKER_COUNT_SAVE;
            if(!File.Exists(countPath)) return;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream countFileStream = new FileStream(countPath, FileMode.Open);
            int count = (int)formatter.Deserialize(countFileStream);
            countFileStream.Close();
            for (int i = 0; i < count; i++)
            {
                if(!File.Exists(path+i)) return;
                
                FileStream stream = new FileStream(path+i, FileMode.Open);
                StickerData data = (StickerData)formatter.Deserialize(stream);
                if(!_itemsSticker.TryGetValue(data.ItemID, out var item)) return;
                var sticker = Instantiate(_stickerPrefab, _parent);
                sticker.transform.localPosition = new Vector2(data.X, data.Y);
                sticker.transform.localScale = new Vector2(data.SizeX, data.SizeY);
                sticker.SetItem(item);
                sticker.SetSprite(item.Sprite);
                _stickers.Add(sticker);
                _stickerCopier.AddSticker(sticker);
                sticker.StartDrag();
                sticker.EndDrag();
                sticker.OnPlaced += OnPlaced;
                sticker.OnReturn += ReturnSticker;
                stream.Close();
            }
        }
        
        private void OnEnable()
        {
            foreach (var items in _stickers)
            {
                items.enabled = true;
            }
        }
        
        private void OnDisable()
        {
            foreach (var items in _stickers)
            {
                items.enabled = false;
            }
        }

        private void InstantiateItem(Item item)
        {
            if(item.Category != _category) return;
            var sticker = Instantiate(_stickerPrefab, _camera.ScreenToWorldPoint(Pointer.current.position.value), Quaternion.identity);
            sticker.SetItem(item);
            _stickers.Add(sticker);
            sticker.SetSprite(item.Sprite);
            sticker.StartDrag();
            sticker.OnPlaced += OnPlaced;
            sticker.OnDragEnd += OnDragEnd;
            sticker.OnReturn += ReturnSticker;
            
        }
        
        private void OnDragEnd(DraggableItem item)
        {
            UnselectSticker(item);
        }
        
        private void OnPlaced(Sticker item)
        {
            _stickerCopier.AddSticker(item);
            SaveStickers();
        }
        
        private void UnselectSticker(DraggableItem item)
        {
            _itemSelector.Unselect(((Sticker)item).Item);
        }

        private void ReturnSticker(DraggableItem item)
        {
            item.OnDragEnd -= UnselectSticker;
            item.OnReturn -= ReturnSticker;
            _stickers.Remove((Sticker)item);
            _stickerCopier.RemoveSticker((Sticker)item);
            Destroy(item.gameObject);
            SaveStickers();
        }


        private void OnDestroy()
        {
            foreach (var sticker in _stickers)
            {
                if(sticker == null) continue;
                sticker.OnDragEnd -= UnselectSticker;
                sticker.OnReturn -= ReturnSticker;
            }
            if(_itemSelector != null)
                _itemSelector.OnItemSelected -= InstantiateItem;
        }
    }
}
