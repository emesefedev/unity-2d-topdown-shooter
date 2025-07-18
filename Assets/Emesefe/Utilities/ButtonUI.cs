using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Emesefe.Utilities
{
    public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler 
    {
        public Action ClickFunc = null;
        public Action MouseRightClickFunc = null;
        public Action MouseMiddleClickFunc = null;
        public Action MouseDownOnceFunc = null;
        public Action MouseUpFunc = null;
        public Action MouseOverOnceTooltipFunc = null;
        public Action MouseOutOnceTooltipFunc = null;
        public Action MouseOverOnceFunc = null;
        public Action MouseOutOnceFunc = null;
        public Action<PointerEventData> OnPointerClickFunc;

        private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;
        public bool hoverBehaviour_Move;
        
        private Vector2 posExit, posEnter;
        public bool triggerMouseOutFuncOnClick = false;
        private bool mouseOver;
        private float mouseOverPerSecFuncTimer;
        
        private Action internalOnPointerEnterFunc = null, internalOnPointerExitFunc = null, internalOnPointerClickFunc = null;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (internalOnPointerEnterFunc != null) internalOnPointerEnterFunc();
            if (hoverBehaviour_Move) transform.GetComponent<RectTransform>().anchoredPosition = posEnter;
            if (hoverBehaviourFunc_Enter != null) hoverBehaviourFunc_Enter();
            if (MouseOverOnceFunc != null) MouseOverOnceFunc();
            if (MouseOverOnceTooltipFunc != null) MouseOverOnceTooltipFunc();
            mouseOver = true;
            mouseOverPerSecFuncTimer = 0f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (internalOnPointerExitFunc != null) internalOnPointerExitFunc();
            if (hoverBehaviour_Move) transform.GetComponent<RectTransform>().anchoredPosition = posExit;
            if (hoverBehaviourFunc_Exit != null) hoverBehaviourFunc_Exit();
            if (MouseOutOnceFunc != null) MouseOutOnceFunc();
            if (MouseOutOnceTooltipFunc != null) MouseOutOnceTooltipFunc();
            mouseOver = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (internalOnPointerClickFunc != null) internalOnPointerClickFunc();
            if (OnPointerClickFunc != null) OnPointerClickFunc(eventData);
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (triggerMouseOutFuncOnClick) {
                    OnPointerExit(eventData);
                }
                if (ClickFunc != null) ClickFunc();
            }
            if (eventData.button == PointerEventData.InputButton.Right)
                if (MouseRightClickFunc != null) MouseRightClickFunc();
            if (eventData.button == PointerEventData.InputButton.Middle)
                if (MouseMiddleClickFunc != null) MouseMiddleClickFunc();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (MouseDownOnceFunc != null) MouseDownOnceFunc();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (MouseUpFunc != null) MouseUpFunc();
        }
    }
}
