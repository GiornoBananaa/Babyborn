using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeDraw
{
    // Helper methods used to set drawing settings
    public class DrawingSettings : MonoBehaviour
    {
        public float Transparency = 1f;
        public int StartWidth = 100;
        public Sprite Texture;

        private void Start()
        {
            SetMarkerWidth(StartWidth);
            ChooseBrush();
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

        public void SetTransparency(float amount)
        {
            Transparency = amount;
            Color c = Drawable.Pen_Colour;
            c.a = amount;
            Drawable.Pen_Colour = c;
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