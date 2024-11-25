using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FreeDraw
{
    // Helper methods used to set drawing settings
    public class DrawingSettings : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _sliderFillImage;
        public float Transparency = 1f;
        public int StartWidth = 100;
        public Sprite Texture;

        private void Start()
        {
            SetMarkerWidth(StartWidth);
            ChooseBrush();
            _slider.onValueChanged.AddListener(SetMarkerWidth);
        }

        // Changing pen settings is easy as changing the static properties Drawable.Pen_Colour and Drawable.Pen_Width
        public void SetMarkerColour(Color new_color)
        {
            Drawable.Pen_Colour = new_color;
            Drawable.drawable.useTexture = false;
        }
        
        // new_width is radius in pixels
        public void SetMarkerWidth(int new_width)
        {
            Drawable.Pen_Width = new_width;
        }
        
        public void SetMarkerWidth(float new_width)
        {
            SetMarkerWidth((int)new_width);
            _sliderFillImage.fillAmount = (new_width-_slider.minValue)/(_slider.maxValue-_slider.minValue);
        }
        
        public void SetTransparency(float amount)
        {
            Transparency = amount;
            Drawable.Transparency = Transparency;
        }
        
        public void SetTexture(Sprite itemSprite)
        {
            Texture = itemSprite;
            Drawable.drawable.texture = itemSprite;
            Drawable.drawable.useTexture = true;
        }

        public void ChooseBrush()
        {
            SetTransparency(Transparency);
            SetTexture(Texture);
        }
        
        public void ChooseEraser()
        {
            SetTransparency(0);
            SetMarkerColour(new Color(255f, 255f, 255f, 0f));
        }

        public void PartialSetEraser()
        {
            SetMarkerColour(new Color(255f, 255f, 255f, 0.5f));
        }

        public void ChooseFillBrush()
        {
            SetTransparency(Transparency);
            Drawable.drawable.SetFillBrush();
        }
    }
}