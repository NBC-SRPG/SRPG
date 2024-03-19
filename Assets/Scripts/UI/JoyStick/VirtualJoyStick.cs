using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image joyStick;
    [SerializeField] private Image controller;
    private Vector2 touchPosition;

    public event Action OnPressJoystick;

    private void Awake()
    {
        joyStick = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        //조이스틱의 위치가 어디에 있든지 동일한 연산을 하기 위한 코드
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joyStick.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            //touchPosition 값 정규화
            touchPosition.x = (touchPosition.x / joyStick.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / joyStick.rectTransform.sizeDelta.y);

            //touchPosition을 중심을 기준으로 정규화
            //왼쪽 좌하단 pivot 기준
            touchPosition = new Vector2(touchPosition.x * 2, touchPosition.y * 2);

            //터치 값이 이미지밖으로 나가도 최대값이 1로 유지되도록
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            controller.rectTransform.anchoredPosition = new Vector2(touchPosition.x * joyStick.rectTransform.sizeDelta.x / 2, touchPosition.y * joyStick.rectTransform.sizeDelta.y / 2);

        }
    }

    //조이스틱 클릭 시
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressJoystick?.Invoke();
    }

    //조이스틱에서 손을 땠을 시
    public void OnPointerUp(PointerEventData eventData)
    {
        controller.rectTransform.anchoredPosition = Vector2.zero;

        touchPosition = Vector2.zero;
    }

    //수평 입력 값
    public float Horizontal()
    {
        return touchPosition.x;
    }

    //수직 입력 값
    public float Vertical()
    {
        return touchPosition.y;
    }
}
