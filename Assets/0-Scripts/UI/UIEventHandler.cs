using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Click 이벤트는 현재 버튼의 OnClick 이벤트를 사용 중
    public Action OnPressedHandler = null;
    public Action OnPointerDownHandler = null;
    public Action OnPointerUpHandler = null;

    private bool pressed = false;

    private void Update()
    {
        if (pressed)
        {
            OnPressedHandler?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        OnPointerDownHandler?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        OnPointerUpHandler?.Invoke();
    }
}
