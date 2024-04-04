using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // 타이틀UI 완성 시 타이틀 UI Show로 교체
        Managers.UI.ShowUI<MainUI>();
        Managers.UI.ShowUI<CommonUI>();
        Debug.Log("Init");
    }
}
