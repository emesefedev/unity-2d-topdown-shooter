using System;
using UnityEngine;
using UnityEngine.UI;

namespace Emesefe.Utilities
{
    public class UISprite
    {
        private GameObject _gameObject;
        private Image _image;
        private RectTransform _rectTransform;

        private static Vector2 _defaultPadding = new Vector2(30, 20);
        private static int _defaultFontSize = 24;

        public UISprite(Transform parent, Sprite sprite, Vector2 anchoredPosition, Vector2 size, Color color) 
        {
            _rectTransform = Utils.DrawSprite(sprite, parent, anchoredPosition, size, "UI Sprite");
            _gameObject = _rectTransform.gameObject;
            
            _image = _gameObject.GetComponent<Image>();
            _image.color = color;
        }
        
        private static Transform GetCanvasTransform() 
        {
            return Utils.GetCanvasTransform();
        }
        
        public static UISprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc) 
        {
            return CreateDebugButton(anchoredPosition, size, ClickFunc, Color.green);
        }

        public static UISprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color) 
        {
            UISprite uiSprite = new UISprite(GetCanvasTransform(), EmesefeAssets.Instance.whiteSprite, anchoredPosition, size, color);
            uiSprite.AddButton(ClickFunc, null, null);
            return uiSprite;
        }

        public static UISprite CreateDebugButton(Transform parent, Vector2 anchoredPosition, string text, Action ClickFunc) 
        {
            return CreateDebugButton(parent, anchoredPosition, text, ClickFunc, Color.green);
        }

        public static UISprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc) 
        {
            return CreateDebugButton(anchoredPosition, text, ClickFunc, Color.green);
        }

        public static UISprite CreateDebugButton(Transform parent, Vector2 anchoredPosition, string text, Action ClickFunc, Color color) 
        {
            return CreateDebugButton(parent, anchoredPosition, text, ClickFunc, color, _defaultPadding);
        }

        public static UISprite CreateDebugButton(Vector2 anchoredPosition, string text, Action ClickFunc, Color color) 
        {
            return CreateDebugButton(GetCanvasTransform(), anchoredPosition, text, ClickFunc, color, _defaultPadding);
        }

        public static UISprite CreateDebugButton(Transform parent, Vector2 anchoredPosition, string text, Action ClickFunc, Color color, Vector2 padding) 
        {
            UISprite uiSprite = CreateDebugButton(parent, anchoredPosition, Vector2.zero, ClickFunc, color, text, out var uiTextTMP);
            uiSprite.SetSize(new Vector2(uiTextTMP.GetTotalWidth(), uiTextTMP.GetTotalHeight()) + padding);
            
            return uiSprite;
        }

        public static UISprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color, string text) 
        {
            UITextTMP uiTextTMPComplex;
            return CreateDebugButton(anchoredPosition, size, ClickFunc, color, text, out uiTextTMPComplex);
        }

        public static UISprite CreateDebugButton(Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color, string text, out UITextTMP uiTextTMP) 
        {
            return CreateDebugButton(GetCanvasTransform(), anchoredPosition, size, ClickFunc, color, text, out uiTextTMP);
        }

        public static UISprite CreateDebugButton(Transform parent, Vector2 anchoredPosition, Vector2 size, Action ClickFunc, Color color, string text, out UITextTMP uiTextTMP) 
        {
            if (color.r >= 1f) color.r = .9f;
            if (color.g >= 1f) color.g = .9f;
            if (color.b >= 1f) color.b = .9f;
            
            Color colorOver = color * 1.1f; // Button Over Color is lighter
            
            UISprite uiSprite = new UISprite(parent, EmesefeAssets.Instance.whiteSprite, anchoredPosition, size, color);
            uiSprite.AddButton(ClickFunc, () => uiSprite.SetColor(colorOver), () => uiSprite.SetColor(color));
            
            uiTextTMP = new UITextTMP(uiSprite._gameObject.transform, Vector2.zero, _defaultFontSize, '#', text, null, null);
            uiTextTMP.SetTextColor(Color.black);
            uiTextTMP.SetAnchorMiddle();
            uiTextTMP.CenterOnPosition(Vector2.zero);
            
            return uiSprite;
        }
        
        public ButtonUI AddButton(Action ClickFunc, Action MouseOverOnceFunc, Action MouseOutOnceFunc) 
        {
            ButtonUI buttonUI = _gameObject.AddComponent<ButtonUI>();
            
            if (ClickFunc != null)
                buttonUI.ClickFunc = ClickFunc;
            
            if (MouseOverOnceFunc != null)
                buttonUI.MouseOverOnceFunc = MouseOverOnceFunc;
            
            if (MouseOutOnceFunc != null)
                buttonUI.MouseOutOnceFunc = MouseOutOnceFunc;
            
            return buttonUI;
        }
        
        public void SetSize(Vector2 size) 
        {
            _rectTransform.sizeDelta = size;
        }
        
        public void SetColor(Color color) 
        {
            _image.color = color;
        }
    }
}