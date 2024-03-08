using UnityEngine;

// UI 관련 스크립트에서 상속하기 위한 Base 클래스
public class UIBase : MonoBehaviour
{
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
