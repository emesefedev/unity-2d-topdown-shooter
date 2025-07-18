using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Emesefe.Utilities
{
    public static class Utils
    {
        private const int SortingOrderDefault = 5000; // To appear in front of anything

        private const int DefaultFontSize = 12;
        private static readonly Color DefaultFontColor = Color.white;

        private static readonly Vector3 DefaultPopupVerticalOffset = new Vector3(0, 10f, 0);
        
        private static readonly Vector2 DefaultReferenceResolution = new Vector2(1920, 1080);

        private static Transform cachedCanvasTransform;

        #region World TMPro

        // Create Text TMPro in the World
        public static TextMeshPro CreateWorldTextTMPro(string text, Transform parent = null,
            Vector3 localPosition = default(Vector3), int fontSize = 12, Color? color = null,
            TextAlignmentOptions textAlignment = TextAlignmentOptions.TopLeft, int sortingOrder = SortingOrderDefault)
        {
            color ??= DefaultFontColor;
            return CreateWorldTextTMPro(parent, text, localPosition, fontSize, (Color)color, textAlignment,
                sortingOrder);
        }

        // Create Text TMPro in the World
        private static TextMeshPro CreateWorldTextTMPro(Transform parent, string text, Vector3 localPosition,
            int fontSize, Color color, TextAlignmentOptions textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World Text", typeof(TextMeshPro));

            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;

            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.alignment = textAlignment;
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color;

            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMeshPro;
        }

        #endregion

        #region TMPro Popup

        // Create a Text TMPro Popup in the World, with no parent
        public static void CreateWorldTextTMProPopup(string text, Vector3 localPosition, float popupTime = 1f)
        {
            CreateWorldTextTMProPopup(null, text, localPosition, DefaultFontSize, DefaultFontColor,
                localPosition + DefaultPopupVerticalOffset, popupTime);
        }

        // Create a Text TMPro Popup in the World
        private static void CreateWorldTextTMProPopup(Transform parent, string text, Vector3 localPosition,
            int fontSize, Color color, Vector3 finalPopupPosition, float popupTime)
        {
            TextMeshPro textMeshPro = CreateWorldTextTMPro(parent, text, localPosition, fontSize, color,
                TextAlignmentOptions.BaselineLeft, SortingOrderDefault);
            Transform transform = textMeshPro.transform;

            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(() =>
            {
                transform.position += moveAmount * Time.unscaledDeltaTime;
                popupTime -= Time.unscaledDeltaTime;

                if (popupTime <= 0f)
                {
                    Object.Destroy(transform.gameObject);
                    return true;
                }

                return false;

            }, "WorldTextTMProPopup");
        }

        #endregion

        #region World Mouse Position

        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePosition = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            mousePosition.z = 0f;
            return mousePosition;
        }

        // Get Mouse Position in World
        private static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera camera)
        {
            Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        #endregion

        // Get Main Canvas Transform
        public static Transform GetCanvasTransform()
        {
            if (cachedCanvasTransform != null) return cachedCanvasTransform;

            Canvas canvas = Object.FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                cachedCanvasTransform = canvas.transform;
            }
            else
            {
                // Create Canvas
                GameObject canvasGameObject = new GameObject("Canvas");
                canvas = canvasGameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
                CanvasScaler canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = DefaultReferenceResolution;
                
                canvasGameObject.AddComponent<GraphicRaycaster>();
                
                cachedCanvasTransform = canvasGameObject.transform;
                
                // Create Event System
                GameObject eventSystemGameObject = new GameObject("Event System");
                eventSystemGameObject.AddComponent<EventSystem>();
                eventSystemGameObject.AddComponent<StandaloneInputModule>();
                eventSystemGameObject.AddComponent<BaseInput>();
            }
            
            return cachedCanvasTransform;
        }

        // Draw a UI Sprite
        public static RectTransform DrawSprite(Color color, Transform parent, Vector2 pos, Vector2 size,
            string name = null)
        {
            RectTransform rectTransform = DrawSprite(null, color, parent, pos, size, name);
            return rectTransform;
        }

        public static RectTransform DrawSprite(Sprite sprite, Transform parent, Vector2 pos, Vector2 size,
            string name = null)
        {
            RectTransform rectTransform = DrawSprite(sprite, Color.white, parent, pos, size, name);
            return rectTransform;
        }

        public static RectTransform DrawSprite(Sprite sprite, Color color, Transform parent, Vector2 pos, Vector2 size,
            string name = null)
        {
            // Setup icon
            if (string.IsNullOrEmpty(name)) name = "Sprite";
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            goRectTransform.SetParent(parent, false);
            goRectTransform.sizeDelta = size;
            goRectTransform.anchoredPosition = pos;

            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;

            return goRectTransform;
        }

        public static TMP_Text DrawTMPTextUI(string textString, Vector2 anchoredPosition, int fontSize, TMP_FontAsset font)
        {
            return DrawTMPTextUI(textString, GetCanvasTransform(), anchoredPosition, fontSize, font);
        }

        public static TMP_Text DrawTMPTextUI(string textString, Transform parent, Vector2 anchoredPosition, int fontSize,
            TMP_FontAsset font)
        {
            GameObject textGameObject = new GameObject("TMP Text", typeof(RectTransform));

            Transform textTransform = textGameObject.transform;
            textTransform.SetParent(parent, false);
            textTransform.localPosition = Vector3.zero;
            textTransform.localScale = Vector3.one;

            RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
            textRectTransform.sizeDelta = new Vector2(0, 0);
            textRectTransform.anchoredPosition = anchoredPosition;

            TextMeshProUGUI text = textGameObject.AddComponent<TextMeshProUGUI>();
            text.text = textString;
            text.overflowMode = TextOverflowModes.Overflow;
            text.enableWordWrapping = false;
            text.alignment = TextAlignmentOptions.MidlineLeft;
            if (font == null) font = GetDefaultFont();
            text.font = font;
            text.fontSize = fontSize;

            return text;
        }

        // Get Default Unity Font, used in text objects if no font given
        public static TMP_FontAsset GetDefaultFont()
        {
            return EmesefeAssets.Instance.defaultFont;
        }
        
        // Parse string to int, return default if failed
        public static int ParseStringToInt(string txt, int defaultInt = -1) {
            if (!int.TryParse(txt, out int i)) {
                i = defaultInt;
            }
            return i;
        }
    }
}
