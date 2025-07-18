using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Emesefe.Utilities
{
    public class UITextTMP
    {
        public struct Icon {
            public Sprite sprite;
            public Vector2 size;
            public Color color;
            public Icon(Sprite sprite, Vector2 size, Color? color = null) {
                this.sprite = sprite;
                this.size = size;
                if (color == null) {
                    this.color = Color.white;
                } else {
                    this.color = (Color) color;
                }
            }
        }
        
        public  GameObject gameObject;
        
        private Transform _transform;
        private RectTransform _rectTransform;

        // iconChar prepends the iconArr index; 
        // Example using iconChar '#': 
        //      test #0 asdf
        // Displays "test [iconArr[0]] asdf"
        public UITextTMP(Transform parent, Vector2 anchoredPosition, int fontSize, char iconChar, string text, Icon[] iconArr, TMP_FontAsset font) 
        {
            SetupParent(parent, anchoredPosition);
            string tmp = text;
            float textPosition = 0f;
            
            while (tmp.IndexOf(iconChar) != -1) {
                // TODO: Understand what this case does
                string untilTmp = tmp.Substring(0, tmp.IndexOf(iconChar));
                string iconNumber = tmp.Substring(tmp.IndexOf(iconChar)+1);
                int indexOfSpaceAfterIconNumber = iconNumber.IndexOf(" ");
                
                if (indexOfSpaceAfterIconNumber != -1) {
                    // Still has more space after iconNumber
                    iconNumber = iconNumber.Substring(0, indexOfSpaceAfterIconNumber);
                } else {
                    // No more space after iconNumber
                }
                tmp = tmp.Substring(tmp.IndexOf(iconChar+iconNumber) + (iconChar+iconNumber).Length);
                if (untilTmp.Trim() != "") {
                    TMP_Text uiText = Utils.DrawTMPTextUI(untilTmp, _transform, new Vector2(textPosition,0), fontSize, font);
                    textPosition += uiText.preferredWidth;
                }
                // Draw Icon
                int iconIndex = Utils.ParseStringToInt(iconNumber, 0);
                Icon icon = iconArr[iconIndex];
                Utils.DrawSprite(icon.sprite, _transform, new Vector2(textPosition + icon.size.x / 2f, 0), icon.size);
                textPosition += icon.size.x;
            }
            
            if (tmp.Trim() != "") {
                Utils.DrawTMPTextUI(tmp, _transform, new Vector2(textPosition,0), fontSize, font);
            }
        }
        
        private void SetupParent(Transform parent, Vector2 anchoredPosition) {
            gameObject = new GameObject("UI Text TMP", typeof(RectTransform));
            _transform = gameObject.transform;
            
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _rectTransform.SetParent(parent, false);
            _rectTransform.sizeDelta = new Vector2(0, 0);
            _rectTransform.anchorMin = new Vector2(0, .5f);
            _rectTransform.anchorMax = new Vector2(0, .5f);
            _rectTransform.pivot = new Vector2(0, .5f);
            _rectTransform.anchoredPosition = anchoredPosition;
        }
        
        public float GetTotalWidth() {
            float textPosition = 0f;
            foreach (Transform trans in _transform) {
                TMP_Text text = trans.GetComponent<TMP_Text>();
                if (text != null) {
                    textPosition += text.preferredWidth;
                }
                Image image = trans.GetComponent<Image>();
                if (image != null) {
                    textPosition += image.GetComponent<RectTransform>().sizeDelta.x;
                }
            }
            return textPosition;
        }
        
        public float GetTotalHeight() {
            foreach (Transform trans in _transform) {
                TMP_Text text = trans.GetComponent<TMP_Text>();
                if (text != null) {
                    return text.preferredHeight;
                }
            }
            return 0f;
        }
        
        public void SetTextColor(Color color) {
            foreach (Transform trans in _transform) {
                TMP_Text text = trans.GetComponent<TMP_Text>();
                if (text != null) {
                    text.color = color;
                }
            }
        }
        
        public void SetAnchorMiddle() {
            _rectTransform.anchorMin = new Vector2(.5f, .5f);
            _rectTransform.anchorMax = new Vector2(.5f, .5f);
        }
        
        public void CenterOnPosition(Vector2 position) {
            _rectTransform.anchoredPosition = position + new Vector2(-GetTotalWidth() / 2f, 0);
        }
    }
}