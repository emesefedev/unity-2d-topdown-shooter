using UnityEngine;
using Emesefe.Utilities;
using System;

namespace Emesefe
{
    public static class EmesefeDebug
    {
        private static readonly Vector3 DefaultOffset = .25f * Vector3.one;
        
        // Text Popup appears on Mouse Position
        public static void TextPopupMouse(string text, Vector3? offset = null)
        {
            offset ??= DefaultOffset;
            Utils.CreateWorldTextTMProPopup(text, Utils.GetMouseWorldPosition() + (Vector3)offset);
        }
        
        // Text Popup at the specified world position
        public static void TextPopup(string text, Vector3 position, float popupTime = 1f) {
            Utils.CreateWorldTextTMProPopup(text, position, popupTime);
        }
        
        // Creates a Button in the UI
        public static UISprite ButtonUI(Vector2 anchoredPosition, string text, Action ClickFunc) {
            return UISprite.CreateDebugButton(anchoredPosition, text, ClickFunc);
        }
        
        public static UISprite ButtonUI(Vector2 anchoredPosition, string text, Action ClickFunc, Color color) {
            return UISprite.CreateDebugButton(anchoredPosition, text, ClickFunc, color);
        }

        public static UISprite ButtonUI(Transform parent, Vector2 anchoredPosition, string text, Action ClickFunc) {
            return UISprite.CreateDebugButton(parent, anchoredPosition, text, ClickFunc);
        }

    }
}
